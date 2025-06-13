using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Repositories;
using Thunders.TechTest.ApiService.Services;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<ICidadeRepository, CidadeRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IFaturamentoHoraCidadeReportRepository, FaturamentoHoraCidadeReportRepository>();
            services.AddScoped<IFaturamentoPracaMesReportRepository, FaturamentoPracaMesReportRepository>();
            services.AddScoped<IFaturamentoPracaTipoVeiculoReportRepository, FaturamentoPracaTipoVeiculoReportRepository>();
            services.AddScoped<IPedagioUtilizacaoRepository, PedagioUtilizacaoRepository>();
            services.AddScoped<IPracaRepository, PracaRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();

            services.AddScoped<IPedagioUtilizacaoService, PedagioUtilizacaoService>();
            services.AddScoped<IMessageSender, RebusMessageSender>();

            return services;
        }
    }
}
