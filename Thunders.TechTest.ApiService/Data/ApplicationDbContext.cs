using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data.Configurations;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<Cidade> CidadeDbSet { get; set; }
        public DbSet<Estado> EstadoDbSet { get; set; }
        public DbSet<FaturamentoHoraCidadeReport> FaturamentoHoraCidadeReportDbSet { get; set; }
        public DbSet<FaturamentoPracaMesReport> FaturamentoPracaMesReportDbSet { get; set; }
        public DbSet<FaturamentoPracaTipoVeiculoReport> FaturamentoPracaTipoVeiculoReportDbSet { get; set; }
        public DbSet<PedagioUtilizacao> PedagioUtilizacaoDbSet { get; set; }
        public DbSet<Praca> PracaDbSet { get; set; }
        public DbSet<Veiculo> VeiculoDbSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CidadeConfiguration());
            modelBuilder.ApplyConfiguration(new EstadoConfiguration());
            modelBuilder.ApplyConfiguration(new FaturamentoHoraCidadeReportConfiguration());
            modelBuilder.ApplyConfiguration(new FaturamentoPracaMesReportConfiguration());
            modelBuilder.ApplyConfiguration(new FaturamentoPracaTipoVeiculoReportConfiguration());
            modelBuilder.ApplyConfiguration(new PedagioUtilizacaoConfiguration());
            modelBuilder.ApplyConfiguration(new PracaConfiguration());
            modelBuilder.ApplyConfiguration(new VeiculoConfiguration());
        }
    }
}
