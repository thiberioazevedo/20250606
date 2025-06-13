using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class PedagioUtilizacaoConfiguration : IEntityTypeConfiguration<PedagioUtilizacao>
    {
        public void Configure(EntityTypeBuilder<PedagioUtilizacao> builder)
        {
            builder.ToTable("PedagiosUtilizacoes");

            builder.HasKey(u => u.Id).HasName("PK_PedagiosUtilizacoes");

            builder.Property(u => u.Id);
            builder.Property(u => u.DataHoraUtilizacao).IsRequired();
            builder.Property(u => u.PracaId);
            builder.Property(u => u.VeiculoId).IsRequired();
            builder.Property(u => u.ValorPago).HasColumnType("DECIMAL(5,2)").IsRequired();
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasIndex(u => new { u.PracaId, u.VeiculoId }).HasDatabaseName("IX_PedagioUtilizacao_PracaId_VeiculoId").IncludeProperties(p => new { p.ValorPago });

            builder.HasIndex(u => new { u.PracaId, u.DataHoraUtilizacao }).HasDatabaseName("IX_PedagioUtilizacao_PracaId_DataHoraUtilizacao").IncludeProperties(p => new { p.ValorPago });

            builder.HasOne(u => u.Praca).WithMany(u => u.PedagioUtilizacaoColletction).HasConstraintName("FK_PedagiosUtilizacoes_Pracas");
            builder.HasOne(u => u.Veiculo).WithMany(u => u.PedagioUtilizacaoColletction).HasConstraintName("FK_PedagiosUtilizacoes_Veiculos");
        }
    }
}
