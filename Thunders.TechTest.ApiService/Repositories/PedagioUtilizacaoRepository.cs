using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class PedagioUtilizacaoRepository : Repository<PedagioUtilizacao>, IPedagioUtilizacaoRepository
    {
        public PedagioUtilizacaoRepository(DbContext context, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider)
        {
        }

        public override Expression<Func<PedagioUtilizacao, bool>>? ExistExpression(PedagioUtilizacao BaseEntity)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<PedagioUtilizacao> GetBaseQuery()
        {
            return base.GetBaseQuery().Include(p => p.Veiculo)
                                      .Include(p => p.Praca).ThenInclude(p => p.Cidade)
                                                            .ThenInclude(c => c.Estado);
        }
    }
}
