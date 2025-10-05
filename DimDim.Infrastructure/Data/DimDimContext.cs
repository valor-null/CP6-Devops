using Microsoft.EntityFrameworkCore;
using DimDim.Core.Entities;

namespace DimDim.Infrastructure.Data;

public class DimDimContext : DbContext
{
    public DimDimContext(DbContextOptions<DimDimContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<ContaCorrente> Contas => Set<ContaCorrente>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DimDimContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}