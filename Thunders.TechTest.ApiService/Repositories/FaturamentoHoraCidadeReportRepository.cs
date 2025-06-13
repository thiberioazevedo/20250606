using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class FaturamentoHoraCidadeReportRepository : BaseReportRepository<FaturamentoHoraCidadeReport>, IFaturamentoHoraCidadeReportRepository
    {
        private readonly IPedagioUtilizacaoRepository pedagioUtilizacaoRepository;

        public FaturamentoHoraCidadeReportRepository(DbContext context, IPedagioUtilizacaoRepository pedagioUtilizacaoRepository, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider)
        {
            this.pedagioUtilizacaoRepository = pedagioUtilizacaoRepository;
        }

        public override Expression<Func<FaturamentoHoraCidadeReport, bool>>? ExistExpression(FaturamentoHoraCidadeReport BaseEntity)
        {
            return i => i.CidadeId == BaseEntity.CidadeId && BaseEntity.Data == i.Data && BaseEntity.Hora == i.Hora;
        }

        public override IQueryable<PedagioUtilizacao> GetPedagioUtilizacaoQuery(FaturamentoHoraCidadeReport faturamentoHoraCidadeReport, CancellationToken cancellationToken)
        {
            var inicio = new DateTime(faturamentoHoraCidadeReport.Data.Year, faturamentoHoraCidadeReport.Data.Month, faturamentoHoraCidadeReport.Data.Day, faturamentoHoraCidadeReport.Hora, 0, 0);
            var fim = inicio.AddHours(1).AddMicroseconds(-2);

            return pedagioUtilizacaoRepository.GetListQuery(i => i.Praca.CidadeId == faturamentoHoraCidadeReport.CidadeId && i.DataHoraUtilizacao >= inicio && i.DataHoraUtilizacao <= fim, null, cancellationToken);
        }

        public override IQueryable<FaturamentoHoraCidadeReport> GetBaseQuery()
        {
            return base.GetBaseQuery().Include(i => i.Cidade).ThenInclude(i => i.Estado);
        }
    }
}
