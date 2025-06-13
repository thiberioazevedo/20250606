using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Thunders.TechTest.ApiService.Services;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Repositories;
using Thunders.TechTest.ApiService.Events;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.Tests.Services
{
    [TestClass]
    public class PedagioUtilizacaoServiceTests
    {
        private Mock<IPedagioUtilizacaoRepository> _pedagioRepoMock;
        private Mock<IFaturamentoHoraCidadeReportRepository> _horaCidadeRepoMock;
        private Mock<IFaturamentoPracaMesReportRepository> _pracaMesRepoMock;
        private Mock<IFaturamentoPracaTipoVeiculoReportRepository> _tipoVeiculoRepoMock;
        private Mock<IPracaRepository> _pracaRepoMock;
        private Mock<ICidadeRepository> _cidadeRepoMock;
        private Mock<IEstadoRepository> _estadoRepoMock;
        private Mock<IVeiculoRepository> _veiculoRepoMock;
        private Mock<IMessageSender> _messageSenderMock;
        private IMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            _pedagioRepoMock = new Mock<IPedagioUtilizacaoRepository>();
            _horaCidadeRepoMock = new Mock<IFaturamentoHoraCidadeReportRepository>();
            _pracaMesRepoMock = new Mock<IFaturamentoPracaMesReportRepository>();
            _tipoVeiculoRepoMock = new Mock<IFaturamentoPracaTipoVeiculoReportRepository>();
            _pracaRepoMock = new Mock<IPracaRepository>();
            _cidadeRepoMock = new Mock<ICidadeRepository>();
            _estadoRepoMock = new Mock<IEstadoRepository>();
            _veiculoRepoMock = new Mock<IVeiculoRepository>();
            _messageSenderMock = new Mock<IMessageSender>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PedagioUtilizacao, PedagioUtilizacaoDTO>();
                cfg.CreateMap<PedagioUtilizacao, PedagioUtilizacaoCriadoEvent>();

                cfg.CreateMap<FaturamentoHoraCidadeReport, FaturamentoHoraCidadeReportDTO>();
                cfg.CreateMap<FaturamentoPracaMesReport, FaturamentoPracaMesReportDTO>();
                cfg.CreateMap<FaturamentoPracaTipoVeiculoReport, FaturamentoPracaTipoVeiculoReportDTO>();
            });

            _mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task RegistrarUtilizacaoAsync_DeveRetornarDTOEsperado()
        {
            // Arrange
            var request = new PedagioUtilizacaoCreateRequestDTO
            {
                Placa = "ABC1234",
                TipoVeiculo = ETipoVeiculo.Carro,
                UF = "SP",
                Cidade = "São Paulo",
                Praca = "Praca A",
                DataHoraUtilizacao = DateTime.UtcNow,
                ValorPago = 15.75m
            };

            var estado = new Estado { Id = Guid.NewGuid(), UF = "SP" };
            var cidade = new Cidade { Id = Guid.NewGuid(), Nome = "São Paulo", EstadoId = estado.Id };
            var praca = new Praca { Id = Guid.NewGuid(), Nome = "Praca A", CidadeId = cidade.Id };
            var veiculo = new Veiculo { Id = Guid.NewGuid(), Placa = "ABC1234", TipoVeiculo = ETipoVeiculo.Carro };

            var pedagio = new PedagioUtilizacao(
                dataHoraUtilizacao: request.DataHoraUtilizacao,
                pracaId: praca.Id,
                veiculoId: veiculo.Id,
                valorPago: request.ValorPago
            );

            // Setup mocks
            _veiculoRepoMock.Setup(r => r.CreateUniqueInstanceAsync(It.IsAny<Veiculo>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                            .ReturnsAsync(veiculo);

            _estadoRepoMock.Setup(r => r.CreateUniqueInstanceAsync(It.IsAny<Estado>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                           .ReturnsAsync(estado);

            _cidadeRepoMock.Setup(r => r.CreateUniqueInstanceAsync(It.IsAny<Cidade>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                           .ReturnsAsync(cidade);

            _pracaRepoMock.Setup(r => r.CreateUniqueInstanceAsync(It.IsAny<Praca>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                           .ReturnsAsync(praca);

            _pedagioRepoMock.Setup(r => r.CreateAsync(It.IsAny<PedagioUtilizacao>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(pedagio);

            var service = new PedagioUtilizacaoService(
                applicationDbContext: null!,
                mapper: _mapper,
                pedagioUtilizacaoRepository: _pedagioRepoMock.Object,
                faturamentoHoraCidadeReportRepository: _horaCidadeRepoMock.Object,
                faturamentoPracaMesReportRepository: _pracaMesRepoMock.Object,
                faturamentoPracaTipoVeiculoReportRepository: _tipoVeiculoRepoMock.Object,
                pracaRepository: _pracaRepoMock.Object,
                cidadeRepository: _cidadeRepoMock.Object,
                estadoRepository: _estadoRepoMock.Object,
                veiculoRepository: _veiculoRepoMock.Object,
                messageSender: _messageSenderMock.Object,
                cacheProvider: Mock.Of<ICacheProvider>()
            );

            // Act
            var result = await service.CreatePedagioUtilizacaoAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(veiculo.Id, result.VeiculoId);
            Assert.AreEqual(praca.Id, result.PracaId);
            Assert.AreEqual(request.ValorPago, result.ValorPago);

            _messageSenderMock.Verify(m => m.SendLocal(It.IsAny<PedagioUtilizacaoCriadoEvent>()), Times.Once);
            _pedagioRepoMock.Verify(r => r.CreateAsync(It.IsAny<PedagioUtilizacao>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ProcessamentoRelatoriosAsync_DeveProcessarETodosOsRelatorios()
        {
            // Arrange
            var horaCidadeReports = new List<FaturamentoHoraCidadeReport> { new(), new() };
            var pracaMesReports = new List<FaturamentoPracaMesReport> { new() };
            var pracaTipoVeiculoReports = new List<FaturamentoPracaTipoVeiculoReport> { new(), new(), new() };

            SetupRepo(_horaCidadeRepoMock, horaCidadeReports);
            SetupRepo(_pracaMesRepoMock, pracaMesReports);
            SetupRepo(_tipoVeiculoRepoMock, pracaTipoVeiculoReports);

            var service = new PedagioUtilizacaoService(
                applicationDbContext: null,
                mapper: _mapper,
                pedagioUtilizacaoRepository: _pedagioRepoMock.Object,
                faturamentoHoraCidadeReportRepository: _horaCidadeRepoMock.Object,
                faturamentoPracaMesReportRepository: _pracaMesRepoMock.Object,
                faturamentoPracaTipoVeiculoReportRepository: _tipoVeiculoRepoMock.Object,
                pracaRepository: _pracaRepoMock.Object,
                cidadeRepository: _cidadeRepoMock.Object,
                estadoRepository: _estadoRepoMock.Object,
                veiculoRepository: _veiculoRepoMock.Object,
                messageSender: _messageSenderMock.Object,
                cacheProvider: Mock.Of<ICacheProvider>()
            );

            var cancellationToken = CancellationToken.None;

            // Act
            var result = await service.ProcessamentoRelatoriosAsync(cancellationToken);

            // Assert
            Assert.AreEqual(2, result.FaturamentoHoraCidadeReportList.Count);
            Assert.AreEqual(1, result.FaturamentoPracaMesReportList.Count);
            Assert.AreEqual(3, result.FaturamentoPracaTipoVeiculoReportList.Count);

            _horaCidadeRepoMock.Verify(x => x.SetReportProcessadoAsync(It.IsAny<FaturamentoHoraCidadeReport>(), cancellationToken), Times.Exactly(2));
            _horaCidadeRepoMock.Verify(x => x.UpdateAsync(It.IsAny<FaturamentoHoraCidadeReport>(), cancellationToken), Times.Exactly(2));

            _pracaMesRepoMock.Verify(x => x.SetReportProcessadoAsync(It.IsAny<FaturamentoPracaMesReport>(), cancellationToken), Times.Once);
            _pracaMesRepoMock.Verify(x => x.UpdateAsync(It.IsAny<FaturamentoPracaMesReport>(), cancellationToken), Times.Once);

            _tipoVeiculoRepoMock.Verify(x => x.SetReportProcessadoAsync(It.IsAny<FaturamentoPracaTipoVeiculoReport>(), cancellationToken), Times.Exactly(3));
            _tipoVeiculoRepoMock.Verify(x => x.UpdateAsync(It.IsAny<FaturamentoPracaTipoVeiculoReport>(), cancellationToken), Times.Exactly(3));
        }


        [TestMethod]
        public async Task ProcessamentoRelatoriosAsync_AtualizaValoresQuantidadeEValorTotalCorretamente()
        {
            // Arrange: crie relatórios com valores iniciais
            var horaCidadeReports = new List<FaturamentoHoraCidadeReport>
            {
                new() { Quantidade = 1, ValorTotal = 10m },
                new() { Quantidade = 2, ValorTotal = 20m }
            };

            // Setup repo para aplicar cálculo no SetValorTotalQuantidadeAsync
            _horaCidadeRepoMock.As<IBaseReportRepository<FaturamentoHoraCidadeReport>>()
                               .Setup(x => x.GetReportsProcessar(It.IsAny<CancellationToken>()))
                               .Returns(horaCidadeReports.AsQueryable());

            _horaCidadeRepoMock.As<IBaseReportRepository<FaturamentoHoraCidadeReport>>()
                               .Setup(x => x.SetReportProcessadoAsync(It.IsAny<FaturamentoHoraCidadeReport>(), It.IsAny<CancellationToken>()))
                               .Returns<FaturamentoHoraCidadeReport, CancellationToken>((report, ct) =>
                               {
                                   // Simula cálculo — por exemplo: aumenta quantidade e valor total
                                   report.Quantidade += 10;
                                   report.ValorTotal += 100m;
                                   return Task.FromResult(report);
                               });

            _horaCidadeRepoMock.As<IBaseReportRepository<FaturamentoHoraCidadeReport>>()
                               .Setup(x => x.UpdateAsync(It.IsAny<FaturamentoHoraCidadeReport>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

            var service = new PedagioUtilizacaoService(
                applicationDbContext: null,
                mapper: _mapper,
                pedagioUtilizacaoRepository: _pedagioRepoMock.Object,
                faturamentoHoraCidadeReportRepository: _horaCidadeRepoMock.Object,
                faturamentoPracaMesReportRepository: _pracaMesRepoMock.Object,
                faturamentoPracaTipoVeiculoReportRepository: _tipoVeiculoRepoMock.Object,
                pracaRepository: _pracaRepoMock.Object,
                cidadeRepository: _cidadeRepoMock.Object,
                estadoRepository: _estadoRepoMock.Object,
                veiculoRepository: _veiculoRepoMock.Object,
                messageSender: _messageSenderMock.Object,
                cacheProvider: Mock.Of<ICacheProvider>()
            );

            // Act
            var result = await service.ProcessamentoRelatoriosAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(2, result.FaturamentoHoraCidadeReportList.Count);

            // Verifica se os valores foram atualizados conforme lógica simulada
            Assert.AreEqual(11, result.FaturamentoHoraCidadeReportList[0].Quantidade);
            Assert.AreEqual(110m, result.FaturamentoHoraCidadeReportList[0].ValorTotal);

            Assert.AreEqual(12, result.FaturamentoHoraCidadeReportList[1].Quantidade);
            Assert.AreEqual(120m, result.FaturamentoHoraCidadeReportList[1].ValorTotal);

            // Verifica se o UpdateAsync foi chamado para cada relatório
            _horaCidadeRepoMock.Verify(x => x.UpdateAsync(It.IsAny<FaturamentoHoraCidadeReport>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task SetValorTotalQuantidadeAsync_DeveAtualizarQuantidadeEValorTotal()
        {
            // Arrange
            var report = new FaturamentoHoraCidadeReport
            {
                Quantidade = 5,
                ValorTotal = 50m
            };

            // Setup para simular cálculo dentro do método SetValorTotalQuantidadeAsync
            _horaCidadeRepoMock.As<IBaseReportRepository<FaturamentoHoraCidadeReport>>()
                               .Setup(x => x.SetReportProcessadoAsync(It.IsAny<FaturamentoHoraCidadeReport>(), It.IsAny<CancellationToken>()))
                               .Returns<FaturamentoHoraCidadeReport, CancellationToken>((r, ct) =>
                               {
                                   // Simula a lógica de atualização dos valores
                                   r.Quantidade += 15; // exemplo de incremento
                                   r.ValorTotal += 150m; // exemplo de incremento
                                   return Task.FromResult(r);
                               });

            var service = new PedagioUtilizacaoService(
                applicationDbContext: null,
                mapper: _mapper,
                pedagioUtilizacaoRepository: _pedagioRepoMock.Object,
                faturamentoHoraCidadeReportRepository: _horaCidadeRepoMock.Object,
                faturamentoPracaMesReportRepository: _pracaMesRepoMock.Object,
                faturamentoPracaTipoVeiculoReportRepository: _tipoVeiculoRepoMock.Object,
                pracaRepository: _pracaRepoMock.Object,
                cidadeRepository: _cidadeRepoMock.Object,
                estadoRepository: _estadoRepoMock.Object,
                veiculoRepository: _veiculoRepoMock.Object,
                messageSender: _messageSenderMock.Object,
                cacheProvider: Mock.Of<ICacheProvider>()
            );

            // Act
            var updatedReport = await _horaCidadeRepoMock.Object.SetReportProcessadoAsync(report, CancellationToken.None);

            // Assert
            Assert.AreEqual(20, updatedReport.Quantidade);
            Assert.AreEqual(200m, updatedReport.ValorTotal);
        }


        private void SetupRepo<T, TRepo>(Mock<TRepo> mockRepo, List<T> reportList) where T : BaseReport where TRepo : class, IBaseReportRepository<T>
        {
            mockRepo.As<IBaseReportRepository<T>>().Setup(x => x.GetReportsProcessar(It.IsAny<CancellationToken>()))
                                                   .Returns(reportList.AsQueryable());

            mockRepo.As<IBaseReportRepository<T>>().Setup(x => x.SetReportProcessadoAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                                                   .ReturnsAsync((T item, CancellationToken _) => item);

            mockRepo.As<IBaseReportRepository<T>>().Setup(x => x.UpdateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                                                   .ReturnsAsync(true);
        }
    }
}
