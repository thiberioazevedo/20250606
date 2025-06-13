using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class FaturamentoPracaTipoVeiculoReportConfiguration : IEntityTypeConfiguration<FaturamentoPracaTipoVeiculoReport>
    {
        public void Configure(EntityTypeBuilder<FaturamentoPracaTipoVeiculoReport> builder)
        {
            builder.ToTable("FaturamentosPracasTiposVeiculosReports");

            builder.HasKey(u => u.Id).HasName("PK_FaturamentosPracasTiposVeiculosReports");

            builder.Property(u => u.Id);
            builder.Property(u => u.PracaId).IsRequired();
            builder.Property(u => u.TipoVeiculo).IsRequired();
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasOne(u => u.Praca).WithMany(u => u.FaturamentoPracaTipoVeiculoReportCollection).HasConstraintName("FK_FaturamentosPracasTiposVeiculosReports_Pracas");

            builder.HasIndex("Processar").IsClustered(false).HasDatabaseName("IX_FaturamentosPracasTiposVeiculosReports_Processar");
        }
    }
}
