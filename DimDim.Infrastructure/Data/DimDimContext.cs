using DimDim.Core.Entities;
using DimDim.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Infrastructure.Data
{
    public class DimDimContext : DbContext
    {
        public DimDimContext(DbContextOptions<DimDimContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ContaCorrente> Contas { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new ContaCorrenteMap());
            modelBuilder.ApplyConfiguration(new TransacaoMap());
        }
    }
}