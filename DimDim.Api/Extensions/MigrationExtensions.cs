using DimDim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Api.Extensions;

public static class MigrationExtensions
{
    public static IApplicationBuilder UseAutoMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<DimDimContext>();
        ctx.Database.Migrate();
        return app;
    }
}