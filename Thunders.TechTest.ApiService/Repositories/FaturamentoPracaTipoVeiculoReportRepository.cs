using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class FaturamentoPracaTipoVeiculoReportRepository : BaseReportRepository<FaturamentoPracaTipoVeiculoReport>, IFaturamentoPracaTipoVeiculoReportRepository
    {
        private readonly IPedagioUtilizacaoRepository pedagioUtilizacaoRepository;

        public FaturamentoPracaTipoVeiculoReportRepository(DbContext context, IPedagioUtilizacaoRepository pedagioUtilizacaoRepository, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider)
        {
            this.pedagioUtilizacaoRepository = pedagioUtilizacaoRepository;
        }

        public override Expression<Func<FaturamentoPracaTipoVeiculoReport, bool>>? ExistExpression(FaturamentoPracaTipoVeiculoReport BaseEntity)
        {
            return i => BaseEntity.PracaId == i.PracaId && BaseEntity.TipoVeiculo == i.TipoVeiculo;
        }

        public override IQueryable<PedagioUtilizacao> GetPedagioUtilizacaoQuery(FaturamentoPracaTipoVeiculoReport faturamentoPracaTipoVeiculoReport, CancellationToken cancellationToken)
        {
            return pedagioUtilizacaoRepository.GetListQuery(
                i => i.PracaId == faturamentoPracaTipoVeiculoReport.PracaId && i.Veiculo.TipoVeiculo == faturamentoPracaTipoVeiculoReport.TipoVeiculo, null, cancellationToken);
        }

        public override IQueryable<FaturamentoPracaTipoVeiculoReport> GetBaseQuery()
        {
            return base.GetBaseQuery().Include(i => i.Praca).ThenInclude(i => i.Cidade).ThenInclude(i => i.Estado);
        }
    }
}
