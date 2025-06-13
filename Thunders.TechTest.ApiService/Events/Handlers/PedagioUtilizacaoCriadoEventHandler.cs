using Rebus.Handlers;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Events.Handlers
{
    public class PedagioUtilizacaoCriadoEventHandler : IHandleMessages<PedagioUtilizacaoCriadoEvent>
    {
        private readonly IFaturamentoHoraCidadeReportRepository faturamentoHoraCidadeReportRepository;
        private readonly IFaturamentoPracaMesReportRepository faturamentoPracaMesReportRepository;
        private readonly IFaturamentoPracaTipoVeiculoReportRepository faturamentoPracaTipoVeiculoReportRepository;
        private readonly ICacheProvider cache;

        public PedagioUtilizacaoCriadoEventHandler(IFaturamentoHoraCidadeReportRepository faturamentoHoraCidadeReportRepository, IFaturamentoPracaMesReportRepository faturamentoPracaMesReportRepository, IFaturamentoPracaTipoVeiculoReportRepository faturamentoPracaTipoVeiculoReportRepository, ICacheProvider cache)
        {
            this.faturamentoHoraCidadeReportRepository = faturamentoHoraCidadeReportRepository;
            this.faturamentoPracaMesReportRepository = faturamentoPracaMesReportRepository;
            this.faturamentoPracaTipoVeiculoReportRepository = faturamentoPracaTipoVeiculoReportRepository;
            this.cache = cache;
        }

        public async Task Handle(PedagioUtilizacaoCriadoEvent pedagioUtilizacaoCriadoEvent)
        {
            try
            {
                var faturamentoHoraCidadeReport = new FaturamentoHoraCidadeReport
                {
                    CidadeId = pedagioUtilizacaoCriadoEvent.CidadeId,
                    Data = pedagioUtilizacaoCriadoEvent.DataHoraUtilizacao.Date,
                    Hora = Convert.ToInt16(pedagioUtilizacaoCriadoEvent.DataHoraUtilizacao.Hour)
                };

                if (!cache.Existe(faturamentoHoraCidadeReport, TimeSpan.FromMinutes(1)))
                {
                    //Console.WriteLine($"Atualizar FaturamentoHoraCidadeReport {pedagioUtilizacaoCriadoEvent.DataHoraUtilizacao:dd/MM/yy:HH}-{pedagioUtilizacaoCriadoEvent.Cidade}-{pedagioUtilizacaoCriadoEvent.UF}");
                    faturamentoHoraCidadeReport = await SetProcessar(faturamentoHoraCidadeReport, faturamentoHoraCidadeReportRepository);
                }

                var faturamentoPracaMesReport = new FaturamentoPracaMesReport
                {
                    Ano = Convert.ToInt16(pedagioUtilizacaoCriadoEvent.DataHoraUtilizacao.Date.Year),
                    Mes = Convert.ToInt16(pedagioUtilizacaoCriadoEvent.DataHoraUtilizacao.Date.Month),
                    PracaId = pedagioUtilizacaoCriadoEvent.PracaId
                };

                if (!cache.Existe(faturamentoPracaMesReport, TimeSpan.FromMinutes(1)))
                {
                    //Console.WriteLine($"Atualizar FaturamentoPracaMesReport {pedagioUtilizacaoCriadoEvent.DataHoraUtilizacao:MM/yyyy}-{pedagioUtilizacaoCriadoEvent.Praca}");
                    faturamentoPracaMesReport = await SetProcessar(faturamentoPracaMesReport, faturamentoPracaMesReportRepository);
                }

                var faturamentoPracaTipoVeiculoReport = new FaturamentoPracaTipoVeiculoReport
                {
                    PracaId = pedagioUtilizacaoCriadoEvent.PracaId,
                    TipoVeiculo = pedagioUtilizacaoCriadoEvent.TipoVeiculo
                };

                if (!cache.Existe(faturamentoPracaTipoVeiculoReport, TimeSpan.FromMinutes(1)))
                {
                    //Console.WriteLine($"Atualizar FaturamentoPracaTipoVeiculoReport {pedagioUtilizacaoCriadoEvent.Praca}-{pedagioUtilizacaoCriadoEvent.TipoVeiculo}");
                    faturamentoPracaTipoVeiculoReport = await SetProcessar(faturamentoPracaTipoVeiculoReport, faturamentoPracaTipoVeiculoReportRepository);
                }

                await Task.CompletedTask;
            }

            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Erro no processamento do evento PedagioUtilizacaoCriadoEvent: {ex.ToString()}");
                }
            }
        }

        private async Task<T> SetProcessar<T>(T baseReport, IBaseReportRepository<T> baseReportRepository) where T: BaseReport
        {
            baseReport = await baseReportRepository.CreateUniqueInstanceAsync(baseReport, null, false);

            if (baseReport.Processar)
                return baseReport;

            baseReport.Processar = true;
            baseReport.InicioProcessamento = null;
            baseReport.FimProcessamento = null;
            baseReport.DataHoraSolicitacao = DateTime.Now;

            if (baseReport.Sequencial > 0)
                await baseReportRepository.UpdateAsync(baseReport);
            else
                await baseReportRepository.CreateAsync(baseReport);

            return baseReport;
        }
    }
}
