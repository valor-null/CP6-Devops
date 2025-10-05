using DimDim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimDim.Infrastructure.Mappings
{
    public class TransacaoMap : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("Transacao");
            builder.HasKey(t => t.IdTransacao);

            builder.Property(t => t.Tipo)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Valor)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(t => t.DataHora)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(t => t.Conta)
                .WithMany(c => c.Transacoes)
                .HasForeignKey(t => t.IdConta);
        }
    }
}