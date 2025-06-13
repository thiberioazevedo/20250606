using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class FaturamentoHoraCidadeReportConfiguration : IEntityTypeConfiguration<FaturamentoHoraCidadeReport>
    {
        public void Configure(EntityTypeBuilder<FaturamentoHoraCidadeReport> builder)
        {
            builder.ToTable("FaturamentosHorasCidadesReports");

            builder.HasKey(u => u.Id).HasName("PK_FaturamentosHorasCidadesReports");

            builder.Property(u => u.Id);
            builder.Property(u => u.CidadeId).IsRequired();
            builder.Property(u => u.Data).HasColumnType("DATE");
            builder.Property(u => u.Hora).IsRequired();
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasOne(u => u.Cidade).WithMany(u => u.FaturamentoHoraCidadeReportCollection).HasConstraintName("FK_FaturamentosHorasCidadesReports_Cidades");

            builder.HasIndex("Processar").IsClustered(false).HasDatabaseName("IX_FaturamentosHorasCidadesReports_Processar");
        }
    }
}
