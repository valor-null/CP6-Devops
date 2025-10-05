using DimDim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimDim.Infrastructure.Mappings;

public class ClienteMap : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Cliente");

        builder.HasKey(x => x.IdCliente);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.CPF)
            .IsRequired()
            .HasColumnType("char(11)");

        builder.HasIndex(x => x.CPF)
            .IsUnique()
            .HasDatabaseName("UQ_Cliente_CPF");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("UQ_Cliente_Email");

        builder.Property(x => x.DataCadastro)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasMany(x => x.Contas)
            .WithOne(x => x.Cliente)
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);
    }
}