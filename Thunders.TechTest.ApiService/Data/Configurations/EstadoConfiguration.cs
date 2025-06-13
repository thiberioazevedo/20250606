using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.ToTable("Estados");

            builder.HasKey(u => u.Id).HasName("PK_Estados");

            builder.Property(u => u.Id);
            builder.Property(u => u.UF).HasColumnType("VARCHAR(2)").IsRequired();
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
