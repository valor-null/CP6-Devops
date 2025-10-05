using DimDim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimDim.Infrastructure.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");
            builder.HasKey(c => c.IdCliente);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CPF)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(c => c.Email)
                .HasMaxLength(150);

            builder.Property(c => c.DataCadastro)
                .HasDefaultValueSql("GETDATE()");

            builder.HasMany(c => c.Contas)
                .WithOne(c => c.Cliente)
                .HasForeignKey(c => c.IdCliente);
        }
    }
}