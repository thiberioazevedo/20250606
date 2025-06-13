using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class PracaConfiguration : IEntityTypeConfiguration<Praca>
    {
        public void Configure(EntityTypeBuilder<Praca> builder)
        {
            builder.ToTable("Pracas");

            builder.HasKey(u => u.Id).HasName("PK_Pracas");

            builder.Property(u => u.Id);
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(u => u.CidadeId).IsRequired();
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasIndex(p => p.CidadeId).HasDatabaseName("IX_Praca_CidadeId");

            builder.HasOne(u => u.Cidade).WithMany(u => u.PracaCollection).HasConstraintName("FK_Cidades_Pracas");
        }
    }
}
