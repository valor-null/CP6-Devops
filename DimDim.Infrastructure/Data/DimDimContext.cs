using Microsoft.EntityFrameworkCore;
using DimDim.Core.Entities;

namespace DimDim.Infrastructure.Data
{
    public class DimDimContext : DbContext
    {
        public DimDimContext(DbContextOptions<DimDimContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ContaCorrente> ContasCorrente { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DimDimContext).Assembly);
        }
    }
}