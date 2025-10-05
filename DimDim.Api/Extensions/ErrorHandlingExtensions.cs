using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DimDim.Api.Extensions;

public static class ErrorHandlingExtensions
{
    public static IServiceCollection AddGlobalProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails();
        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                var status = exception is ArgumentException ? StatusCodes.Status400BadRequest :
                    exception is InvalidOperationException ? StatusCodes.Status409Conflict :
                    StatusCodes.Status500InternalServerError;

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = "Erro ao processar a requisição",
                    Detail = exception?.Message
                };

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            });
        });

        return app;
    }
}