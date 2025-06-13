using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.Pagination;

namespace Thunders.TechTest.ApiService.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<ICollection<T>> UpdateRangeAsync(ICollection<T> entityCollection, CancellationToken cancellationToken);
        Task CreateRangeAsync(IEnumerable<T> pedagioUtilizacoes, CancellationToken cancellationToken);
        Task<ICollection<T>> CreateRangeAsync(ICollection<T> entityCollection, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedList<T>> GetPaginatedList(Expression<Func<T, bool>>? expression, string? searchText, string? orderByStringList, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
        IQueryable<T> GetListQuery(Expression<Func<T, bool>>? expression, string? searchText, CancellationToken? cancellationToken);
        abstract Expression<Func<T, bool>>? ExistExpression(T BaseEntity);
        Task<T> CreateUniqueInstanceAsync(T baseEntity, CancellationToken? cancellationToken, bool create = true);
        IQueryable<T> GetBaseQuery();
    }
    public interface IBaseReportRepository<T> : IRepository<T> where T : BaseReport
    {
        public IQueryable<T> GetReportsProcessar(CancellationToken cancellationToken = default);
        Task<T> SetReportProcessadoAsync(T baseReport, CancellationToken cancellationToken = default);
        public abstract IQueryable<PedagioUtilizacao> GetPedagioUtilizacaoQuery(T baseReport, CancellationToken cancellationToken);
    }
}