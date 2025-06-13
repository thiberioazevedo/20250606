using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.Repositories;

namespace Thunders.TechTest.Tests.Repositories
{
    [TestClass]
    public class FaturamentoPracaMesReportRepositoryTests
    {
        private FaturamentoPracaMesReportRepository _repository;
        private ApplicationDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var cidadeId = Guid.NewGuid();
            var pracaId = Guid.NewGuid();
            var veiculoId = Guid.NewGuid();

            _context.PedagioUtilizacaoDbSet.Add(new PedagioUtilizacao
            {
                ValorPago = 200m,
                DataHoraUtilizacao = new DateTime(2025, 1, 15),
                Praca = new Praca
                {
                    Id = pracaId,
                    Nome = "Praça Teste",
                    CidadeId = cidadeId
                },
                VeiculoId = veiculoId
            });

            _context.SaveChanges();

            var mockRepo = new Mock<IPedagioUtilizacaoRepository>();
            mockRepo.Setup(x => x.GetListQuery(It.IsAny<Expression<Func<PedagioUtilizacao, bool>>>(), null, It.IsAny<CancellationToken>()))
                    .Returns((Expression<Func<PedagioUtilizacao, bool>> filtro, object _, CancellationToken __) =>
                        _context.PedagioUtilizacaoDbSet.Where(filtro));

            _repository = new FaturamentoPracaMesReportRepository(_context, mockRepo.Object, null);
        }

        [TestMethod]
        public async Task SetValorTotalQuantidadeAsync_DeveAtualizarQuantidadeEValorTotal_Corretamente()
        {
            var report = new FaturamentoPracaMesReport
            {
                Ano = 2025,
                Mes = 1,
                PracaId = _context.PedagioUtilizacaoDbSet.First().Praca.Id,
                Processar = true
            };

            var result = await _repository.SetReportProcessadoAsync(report, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Quantidade);
            Assert.AreEqual(200m, result.ValorTotal);
            Assert.IsFalse(result.Processar);
            Assert.IsTrue(result.InicioProcessamento <= result.FimProcessamento);
        }
    }
}
