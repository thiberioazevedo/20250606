using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.Repositories;
using Thunders.TechTest.ApiService.Data;
using Moq;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;

namespace Thunders.TechTest.Tests.Repositories
{
    [TestClass]
    public class FaturamentoHoraCidadeReportRepositoryTests
    {
        private FaturamentoHoraCidadeReportRepository _repository;
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private ApplicationDbContext context;

        [TestInitialize]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                                                   .Options;   

            context = new ApplicationDbContext(_dbContextOptions);

            // Populando dado necessário
            var cidadeId = Guid.NewGuid();
            var pracaId = Guid.NewGuid();
            var veiculoId = Guid.NewGuid();

            context.PedagioUtilizacaoDbSet.Add(new PedagioUtilizacao
            {
                ValorPago = 100m,
                DataHoraUtilizacao = new DateTime(2025, 1, 1, 10, 0, 0),
                Praca = new Praca
                {
                    Id = pracaId,
                    Nome = "Nome da Praça",
                    CidadeId = cidadeId
                },
                VeiculoId = veiculoId
            });

            context.SaveChanges();

            // Mock do IPedagioUtilizacaoRepository para retornar os dados da query
            var pedagioRepoMock = new Mock<IPedagioUtilizacaoRepository>();
            pedagioRepoMock.Setup(x => x.GetListQuery(It.IsAny<Expression<Func<PedagioUtilizacao, bool>>>(), null, It.IsAny<CancellationToken>()))
                           .Returns((Expression<Func<PedagioUtilizacao, bool>> filtro, object _, CancellationToken __) =>
                               context.PedagioUtilizacaoDbSet.Where(filtro));

            _repository = new FaturamentoHoraCidadeReportRepository(context, pedagioRepoMock.Object, null);
        }


        [TestMethod]
        public async Task SetValorTotalQuantidadeAsync_DeveAtualizarQuantidadeEValorTotal_Corretamente()
        {
            // Arrange
            var report = new FaturamentoHoraCidadeReport
            {
                Data = new DateTime(2025, 1, 1),
                Hora = 10,
                CidadeId = context.PedagioUtilizacaoDbSet.First().Praca.CidadeId,
                Processar = true
            };

            // Act
            var resultado = await _repository.SetReportProcessadoAsync(report, CancellationToken.None);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Quantidade);
            Assert.AreEqual(100m, resultado.ValorTotal);
            Assert.IsFalse(resultado.Processar);
            Assert.IsTrue(resultado.InicioProcessamento <= resultado.FimProcessamento);
        }

    }
}
