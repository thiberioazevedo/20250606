using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("Veiculos");

            builder.HasKey(u => u.Id).HasName("PK_Veiculos");

            builder.Property(u => u.Id);
            builder.Property(u => u.Placa).HasColumnType("VARCHAR(7)").IsRequired();
            builder.Property(u => u.TipoVeiculo).IsRequired();
            builder.Property(u => u.Sequencial).ValueGeneratedOnAdd().UseIdentityColumn(seed: 1, increment: 1).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasIndex(v => new { v.TipoVeiculo, v.Id }).HasDatabaseName("IX_Veiculo_TipoVeiculo_Id");

            builder.HasIndex(v => v.Placa).HasDatabaseName("IX_Veiculo_Placa");
        }
    }
}
