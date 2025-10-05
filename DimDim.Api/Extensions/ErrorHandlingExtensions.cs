using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
                var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                var status = StatusCodes.Status500InternalServerError;
                var detail = ex?.Message;

                if (ex is DbUpdateConcurrencyException)
                {
                    status = StatusCodes.Status409Conflict;
                    detail = "Conflito de concorrência.";
                }
                else if (ex is DbUpdateException dbex && dbex.InnerException is SqlException sql)
                {
                    if (sql.Number is 2627 or 2601)
                    {
                        status = StatusCodes.Status409Conflict;
                        detail = "Registro duplicado.";
                    }
                    else if (sql.Number == 547)
                    {
                        var msg = sql.Message ?? string.Empty;
                        if (msg.Contains("CHECK", StringComparison.OrdinalIgnoreCase))
                        {
                            status = StatusCodes.Status400BadRequest;
                            detail = "Violação de regra de negócio (CHECK).";
                        }
                        else
                        {
                            status = StatusCodes.Status409Conflict;
                            detail = "Violação de integridade referencial.";
                        }
                    }
                    else
                    {
                        status = StatusCodes.Status400BadRequest;
                    }
                }
                else if (ex is ArgumentException)
                {
                    status = StatusCodes.Status400BadRequest;
                }
                else if (ex is InvalidOperationException)
                {
                    status = StatusCodes.Status409Conflict;
                }

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = "Erro ao processar a requisição",
                    Detail = detail
                };

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            });
        });

        return app;
    }
}
