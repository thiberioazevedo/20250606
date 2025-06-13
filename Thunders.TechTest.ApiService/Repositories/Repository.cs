using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Pagination;
using Thunders.TechTest.ApiService.Models;
using System.Collections.ObjectModel;

namespace Thunders.TechTest.ApiService.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext dbContext;
        private readonly IEntityCacheProvider entityCacheProvider;

        internal DbSet<T> DbSet
        {
            get
            {
                return dbContext.Set<T>();
            }
        }

        public Repository(DbContext context, IEntityCacheProvider entityCacheProvider)
        {
            dbContext = context;
            this.entityCacheProvider = entityCacheProvider;
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task<ICollection<T>> CreateRangeAsync(ICollection<T> entityCollection, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entityCollection, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entityCollection;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery().Where(o => o.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return false;

            DbSet.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public virtual IQueryable<T> GetBaseQuery()
        {
            return DbSet;
        }

        public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery().ToListAsync();
        }

        public virtual IQueryable<T> GetListQuery(Expression<Func<T, bool>>? expression, string? searchText, CancellationToken? cancellationToken)
        {
            if (expression == null)
                return GetBaseQuery();

            return GetBaseQuery().Where(expression);
        }

        internal virtual IQueryable<T> OrderBy(IQueryable<T> queryable, string? orderByStringList)
        {
            if (string.IsNullOrEmpty(orderByStringList))
                return queryable;

            var orderByList = orderByStringList.Split('|')
                                               .Select(i => i.Split(','))
                                               .ToDictionary(i => i[0], i => (i.Length < 2 ? null : i[1]) == "desc");

            queryable = queryable.OrderBy(string.Join(",", orderByList.Select(i => i.Key + " " + (i.Value ? "desc" : "asc"))));

            return queryable;
        }

        public async Task<PaginatedList<T>> GetPaginatedList(Expression<Func<T, bool>>? expression, string? searchText, string? orderByStringList, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var queryable = GetListQuery(expression, searchText, cancellationToken);

            queryable = OrderBy(queryable, orderByStringList);

            var pagination = await PaginatedList<T>.CreateInstanceAsync(pageNumber, pageSize, queryable);

            return await Task.FromResult(pagination);
        }

        public abstract Expression<Func<T, bool>>? ExistExpression(T BaseEntity);

        public async Task<T> CreateUniqueInstanceAsync(T entity, CancellationToken? cancellationToken, bool create = true) {
            var entityCache = entityCacheProvider.GetInstance(entity, this);

            if (!ReferenceEquals(entityCache, entity))
                return entityCache;
            
            if (create)
                await CreateAsync(entity);

            return entity;
        }

        public async Task<ICollection<T>> UpdateRangeAsync(ICollection<T> entityCollection, CancellationToken cancellationToken)
        {
            DbSet.UpdateRange(entityCollection);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entityCollection;
        }
    }
}
