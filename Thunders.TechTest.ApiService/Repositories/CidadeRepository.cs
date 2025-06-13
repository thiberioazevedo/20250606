using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class CidadeRepository : Repository<Cidade>, ICidadeRepository
    {
        public CidadeRepository(DbContext context, IEntityCacheProvider entityCacheProvider ) : base(context, entityCacheProvider)
        {
        }

        public override Expression<Func<Cidade, bool>>? ExistExpression(Cidade cidade)
        {
            return i => i.Nome == cidade.Nome && i.EstadoId == cidade.EstadoId;
        }
    }
}
