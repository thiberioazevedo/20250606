using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(DbContext context, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider)
        {
        }

        public override Expression<Func<Veiculo, bool>>? ExistExpression(Veiculo veiculo)
        {
            return i => i.Placa == veiculo.Placa;
        }
    }
}
