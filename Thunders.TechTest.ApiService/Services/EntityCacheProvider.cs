using System.Linq;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Services
{
    public class EntityCacheProvider : IEntityCacheProvider
    {
        public EntityCacheProvider()
        {
            Dictionary = new Dictionary<Type, IList<object>>();
        }

        public Dictionary<Type, IList<object>> Dictionary { get; internal set; }

        public T GetInstance<T>(T entity, IRepository<T> repository) where T : BaseEntity
        {
            var type = entity.GetType();

            if (!Dictionary.ContainsKey(type))
                Dictionary.Add(type, new List<object>());

            var list = Dictionary[type];

            var expression = repository.ExistExpression(entity);

            var entity_ = list.Select(i => (T)i).AsQueryable().Where(expression).FirstOrDefault();

            if (entity_ != null)
                return entity_;

            entity_ = repository.GetBaseQuery().Where(expression).FirstOrDefault();

            if (entity_ != null)
            {
                list.Add(entity_);
                return entity_;
            }
            else { 
                list.Add(entity);
                return entity;
            }
        }
    }
}
