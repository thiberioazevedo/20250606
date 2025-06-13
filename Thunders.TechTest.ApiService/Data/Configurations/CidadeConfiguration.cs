using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ToTable("Cidades");

            builder.HasKey(u => u.Id).HasName("PK_Cidades");

            builder.Property(u => u.Id);
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(u => u.EstadoId);
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasOne(u => u.Estado).WithMany(u => u.CidadeCollection).HasConstraintName("FK_Cidades_Estados");
        }
    }
}
