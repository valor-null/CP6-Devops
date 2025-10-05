using DimDim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimDim.Infrastructure.Mappings
{
    public class ContaCorrenteMap : IEntityTypeConfiguration<ContaCorrente>
    {
        public void Configure(EntityTypeBuilder<ContaCorrente> builder)
        {
            builder.ToTable("ContaCorrente");
            builder.HasKey(c => c.IdConta);

            builder.Property(c => c.NumeroConta)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.Saldo)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.TipoConta)
                .HasMaxLength(50)
                .HasDefaultValue("Corrente");

            builder.HasOne(c => c.Cliente)
                .WithMany(c => c.Contas)
                .HasForeignKey(c => c.IdCliente);

            builder.HasMany(c => c.Transacoes)
                .WithOne(t => t.Conta)
                .HasForeignKey(t => t.IdConta);
        }
    }
}