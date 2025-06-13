using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class PracaRepository : Repository<Praca>, IPracaRepository
    {
        public PracaRepository(DbContext context, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider)
        {
        }

        public override Expression<Func<Praca, bool>>? ExistExpression(Praca praca)
        {
            return i => i.Nome == praca.Nome && i.CidadeId == praca.CidadeId;
        }
    }
}
