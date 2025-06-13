using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Interfaces
{
    public interface IEntityCacheProvider
    {
        T GetInstance<T>(T entity, IRepository<T> repository) where T : BaseEntity;
    }
}
