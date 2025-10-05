using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DimDim.Infrastructure.Data;

public class DesignTimeDimDimContextFactory : IDesignTimeDbContextFactory<DimDimContext>
{
    public DimDimContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DimDimContext>()
            .UseSqlServer(
                Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? "Server=localhost,1433;Database=DimDimDB;User Id=sa;Password=Senha123!;TrustServerCertificate=True;Encrypt=False;")
            .Options;

        return new DimDimContext(options);
    }
}