using DimDim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DimDim.Infrastructure.Mappings;

public class TransacaoMap : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("Transacao");

        builder.HasKey(x => x.IdTransacao);

        builder.Property(x => x.Tipo)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasCheckConstraint("CK_Transacao_Tipo", "Tipo IN ('CREDITO','DEBITO','TRANSFERENCIA')");

        builder.Property(x => x.Valor)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasCheckConstraint("CK_Transacao_Valor", "Valor > 0");

        builder.Property(x => x.DataHora)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(x => new { x.IdConta, x.DataHora })
            .IsDescending(false, true)
            .HasDatabaseName("IX_Transacao_IdConta_DataHora");

        builder.HasOne(x => x.Conta)
            .WithMany(x => x.Transacoes)
            .HasForeignKey(x => x.IdConta)
            .OnDelete(DeleteBehavior.Restrict);
    }
}