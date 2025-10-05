using DimDim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimDim.Infrastructure.Mappings;

public class ContaCorrenteMap : IEntityTypeConfiguration<ContaCorrente>
{
    public void Configure(EntityTypeBuilder<ContaCorrente> builder)
    {
        builder.ToTable("ContaCorrente");

        builder.HasKey(x => x.IdConta);

        builder.Property(x => x.NumeroConta)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.NumeroConta)
            .IsUnique()
            .HasDatabaseName("UQ_ContaCorrente_NumeroConta");

        builder.Property(x => x.Saldo)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(x => x.TipoConta)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasCheckConstraint("CK_ContaCorrente_TipoConta", "TipoConta IN ('Corrente','Poupanca')");

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(x => x.IdCliente)
            .HasDatabaseName("IX_Conta_IdCliente");

        builder.HasOne(x => x.Cliente)
            .WithMany(x => x.Contas)
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Transacoes)
            .WithOne(x => x.Conta)
            .HasForeignKey(x => x.IdConta)
            .OnDelete(DeleteBehavior.Restrict);
    }
}