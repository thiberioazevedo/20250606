using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class EstadoRepository : Repository<Estado>, IEstadoRepository
    {
        public EstadoRepository(DbContext context, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider)
        {
        }

        public override Expression<Func<Estado, bool>>? ExistExpression(Estado estado)
        {
            if (!string.IsNullOrEmpty(estado.UF))
                return i => i.UF == estado.UF;

            return i => i.Nome == estado.Nome;
        }
    }
}
