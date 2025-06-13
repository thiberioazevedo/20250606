using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class FaturamentoPracaMesReportRepository : BaseReportRepository<FaturamentoPracaMesReport>, IFaturamentoPracaMesReportRepository
    {
        private readonly IPedagioUtilizacaoRepository pedagioUtilizacaoRepository;

        public FaturamentoPracaMesReportRepository(DbContext context, IPedagioUtilizacaoRepository pedagioUtilizacaoRepository, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider)
        {
            this.pedagioUtilizacaoRepository = pedagioUtilizacaoRepository;
        }

        public override Expression<Func<FaturamentoPracaMesReport, bool>>? ExistExpression(FaturamentoPracaMesReport BaseEntity)
        {
            return i => BaseEntity.PracaId == i.PracaId && BaseEntity.Ano == i.Ano && i.Mes == BaseEntity.Mes;
        }

        public override IQueryable<PedagioUtilizacao> GetPedagioUtilizacaoQuery(FaturamentoPracaMesReport faturamentoPracaMesReport, CancellationToken cancellationToken)
        {
            var inicio = new DateTime(faturamentoPracaMesReport.Ano, faturamentoPracaMesReport.Mes, 1);
            var fim = inicio.AddMonths(1).AddMicroseconds(-2);

            return pedagioUtilizacaoRepository.GetListQuery(i => i.PracaId == faturamentoPracaMesReport.PracaId && i.DataHoraUtilizacao >= inicio && i.DataHoraUtilizacao <= fim, null, cancellationToken);
        }

        public override IQueryable<FaturamentoPracaMesReport> GetBaseQuery()
        {
            return base.GetBaseQuery().Include(i => i.Praca).ThenInclude(i => i.Cidade).ThenInclude(i => i.Estado);
        }
    }
}
