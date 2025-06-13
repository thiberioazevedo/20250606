using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class FaturamentoPracaMesReportConfiguration : IEntityTypeConfiguration<FaturamentoPracaMesReport>
    {
        public void Configure(EntityTypeBuilder<FaturamentoPracaMesReport> builder)
        {
            builder.ToTable("FaturamentosPracasMesesReports");

            builder.HasKey(u => u.Id).HasName("PK_FaturamentosPracasMesesReports");

            builder.Property(u => u.Id);
            builder.Property(u => u.PracaId).IsRequired();
            builder.Property(u => u.Ano).IsRequired();
            builder.Property(u => u.Mes).IsRequired();
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasOne(u => u.Praca).WithMany(u => u.FaturamentoPracaMesReportCollection).HasConstraintName("FK_FaturamentosPracasMesesReports_Pracas");

            builder.HasIndex("Processar").IsClustered(false).HasDatabaseName("IX_FaturamentosPracasMesesReports_Processar");
        }
    }
}
