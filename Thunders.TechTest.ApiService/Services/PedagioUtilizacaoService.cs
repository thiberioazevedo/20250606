using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Events;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.Pagination;
using Thunders.TechTest.ApiService.Repositories;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Services
{
    public class PedagioUtilizacaoService : IPedagioUtilizacaoService
    {
        private readonly IMapper mapper;
        private readonly IPedagioUtilizacaoRepository pedagioUtilizacaoRepository;
        private readonly IFaturamentoHoraCidadeReportRepository faturamentoHoraCidadeReportRepository;
        private readonly IFaturamentoPracaMesReportRepository faturamentoPracaMesReportRepository;
        private readonly IFaturamentoPracaTipoVeiculoReportRepository faturamentoPracaTipoVeiculoReportRepository;
        private readonly IPracaRepository pracaRepository;
        private readonly ICidadeRepository cidadeRepository;
        private readonly IEstadoRepository estadoRepository;
        private readonly IVeiculoRepository veiculoRepository;
        private readonly IMessageSender messageSender;
        private readonly ICacheProvider cacheProvider;

        public PedagioUtilizacaoService(ApplicationDbContext applicationDbContext, IMapper mapper, IPedagioUtilizacaoRepository pedagioUtilizacaoRepository, IFaturamentoHoraCidadeReportRepository faturamentoHoraCidadeReportRepository, IFaturamentoPracaMesReportRepository faturamentoPracaMesReportRepository, IFaturamentoPracaTipoVeiculoReportRepository faturamentoPracaTipoVeiculoReportRepository, IPracaRepository pracaRepository, ICidadeRepository cidadeRepository, IEstadoRepository estadoRepository, IVeiculoRepository veiculoRepository, IMessageSender messageSender, ICacheProvider cacheProvider)
        {
            this.mapper = mapper;
            this.pedagioUtilizacaoRepository = pedagioUtilizacaoRepository;
            this.faturamentoHoraCidadeReportRepository = faturamentoHoraCidadeReportRepository;
            this.faturamentoPracaMesReportRepository = faturamentoPracaMesReportRepository;
            this.faturamentoPracaTipoVeiculoReportRepository = faturamentoPracaTipoVeiculoReportRepository;
            this.pracaRepository = pracaRepository;
            this.cidadeRepository = cidadeRepository;
            this.estadoRepository = estadoRepository;
            this.veiculoRepository = veiculoRepository;
            this.messageSender = messageSender;
            this.cacheProvider = cacheProvider;
        }

        private async Task<(PedagioUtilizacao pedagioUtilizacao, PedagioUtilizacaoCriadoEvent pedagioUtilizacaoCriadoEvent)> CriarUtilizacaoComEventoAsync(
            PedagioUtilizacaoCreateRequestDTO pedagioUtilizacaoCreateRequestDTO, CancellationToken cancellationToken)
        {
            var veiculo = await veiculoRepository.CreateUniqueInstanceAsync(new Veiculo { Placa = pedagioUtilizacaoCreateRequestDTO.Placa, TipoVeiculo = pedagioUtilizacaoCreateRequestDTO.TipoVeiculo }, cancellationToken);

            var estado = await estadoRepository.CreateUniqueInstanceAsync(new Estado { UF = pedagioUtilizacaoCreateRequestDTO.UF, Nome = pedagioUtilizacaoCreateRequestDTO.UF }, cancellationToken);

            var cidade = await cidadeRepository.CreateUniqueInstanceAsync(new Cidade { Nome = pedagioUtilizacaoCreateRequestDTO.Cidade, EstadoId = estado.Id }, cancellationToken);

            var praca = await pracaRepository.CreateUniqueInstanceAsync(new Praca { Nome = pedagioUtilizacaoCreateRequestDTO.Praca, CidadeId = cidade.Id }, cancellationToken);

            var pedagioUtilizacao = new PedagioUtilizacao(pedagioUtilizacaoCreateRequestDTO.DataHoraUtilizacao, praca.Id, veiculo.Id, pedagioUtilizacaoCreateRequestDTO.ValorPago);

            var pedagioUtilizacaoCriadoEvent = mapper.Map<PedagioUtilizacaoCriadoEvent>(pedagioUtilizacaoCreateRequestDTO);
            pedagioUtilizacaoCriadoEvent.CidadeId = cidade.Id;
            pedagioUtilizacaoCriadoEvent.PracaId = praca.Id;
            pedagioUtilizacaoCriadoEvent.VeiculoId = veiculo.Id;

            return (pedagioUtilizacao, pedagioUtilizacaoCriadoEvent);
        }

        public async Task<IList<PedagioUtilizacaoDTO>> CreatePedagioUtilizacaoRangeAsync(IList<PedagioUtilizacaoCreateRequestDTO> pedagioUtilizacaoCreateRequestDTOList, CancellationToken cancellationToken)
        {
            var pedagioUtilizacaoList = new List<PedagioUtilizacao>();
            var pedagioUtilizacaoCriadoEventList = new List<PedagioUtilizacaoCriadoEvent>();

            foreach (var pedagioUtilizacaoCreateRequestDTO in pedagioUtilizacaoCreateRequestDTOList)
            {
                var (pedagioUtilizacao, pedagioUtilizacaoCriadoEvent) = await CriarUtilizacaoComEventoAsync(pedagioUtilizacaoCreateRequestDTO, cancellationToken);
                pedagioUtilizacaoList.Add(pedagioUtilizacao);
                pedagioUtilizacaoCriadoEventList.Add(pedagioUtilizacaoCriadoEvent);
            }

            await pedagioUtilizacaoRepository.CreateRangeAsync(pedagioUtilizacaoList, cancellationToken);

            foreach (var pedagioUtilizacaoCriadoEvent in pedagioUtilizacaoCriadoEventList)
            {
                _ = messageSender.SendLocal(pedagioUtilizacaoCriadoEvent);
            }

            return mapper.Map<IList<PedagioUtilizacaoDTO>>(pedagioUtilizacaoList);
        }


        public async Task<PedagioUtilizacaoDTO> CreatePedagioUtilizacaoAsync(PedagioUtilizacaoCreateRequestDTO dto, CancellationToken cancellationToken)
        {
            var (utilizacao, evento) = await CriarUtilizacaoComEventoAsync(dto, cancellationToken);

            await pedagioUtilizacaoRepository.CreateAsync(utilizacao, cancellationToken);

            _ = messageSender.SendLocal(evento);

            return mapper.Map<PedagioUtilizacaoDTO>(utilizacao);
        }

        public async Task<ProcessamentoRelatoriosDTO> ProcessamentoRelatoriosAsync(CancellationToken cancellationToken)
        {
            var processamentoRelatoriosDTO = new ProcessamentoRelatoriosDTO();

            var faturamentoHoraCidadeReportList = (await ProcessamentoRelatoriosAsync(faturamentoHoraCidadeReportRepository, cancellationToken)).ToList();
            processamentoRelatoriosDTO.FaturamentoHoraCidadeReportList = mapper.Map<List<FaturamentoHoraCidadeReportDTO>>(faturamentoHoraCidadeReportList);

            var faturamentoPracaMesReportList = (await ProcessamentoRelatoriosAsync(faturamentoPracaMesReportRepository, cancellationToken)).ToList();
            processamentoRelatoriosDTO.FaturamentoPracaMesReportList = mapper.Map<List<FaturamentoPracaMesReportDTO>>(faturamentoPracaMesReportList);

            var faturamentoPracaTipoVeiculoReportList = (await ProcessamentoRelatoriosAsync(faturamentoPracaTipoVeiculoReportRepository, cancellationToken)).ToList();
            processamentoRelatoriosDTO.FaturamentoPracaTipoVeiculoReportList = mapper.Map<List<FaturamentoPracaTipoVeiculoReportDTO>>(faturamentoPracaTipoVeiculoReportList);

            return processamentoRelatoriosDTO;
        }

        private async Task<IList<T>> ProcessamentoRelatoriosAsync<T>(IBaseReportRepository<T> baseReportRepository, CancellationToken cancellationToken) where T : BaseReport
        {
            var baseReportList = baseReportRepository.GetReportsProcessar().OrderBy(i => i.DataHoraSolicitacao).Take(10000).ToList();

            const int batchSize = 50;
            for (int i = 0; i < baseReportList.Count; i += batchSize)
            {
                var baseReportListBatch = baseReportList.Skip(i).Take(batchSize).ToList();
                var taskList = new List<Task>();

                foreach (var baseReport in baseReportListBatch)
                {
                    var id = baseReport.Id;
                    baseReport.Id = Guid.Empty;
                    cacheProvider.Remover(baseReport);
                    baseReport.Id = id;

                    taskList.Add(baseReportRepository.SetReportProcessadoAsync(baseReport, cancellationToken));
                }

                await Task.WhenAll(taskList);

                await baseReportRepository.UpdateRangeAsync(baseReportListBatch, cancellationToken);
            }

            return baseReportList;
        }

        public async Task<PaginatedList<PedagioUtilizacaoDTO>> GetPaginatedList(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return (await pedagioUtilizacaoRepository.GetPaginatedList(null, null, null, pageNumber, pageSize, cancellationToken)).Map<PedagioUtilizacaoDTO>(mapper);
        }
    }
}
