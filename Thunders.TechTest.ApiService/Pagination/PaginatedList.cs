using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Thunders.TechTest.ApiService.Pagination
{
    public class PaginatedRequestBaseList()
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public static PaginatedList<TDestination> MapPaginationList<TSource, TDestination>(PaginatedList<TSource> response, IMapper mapper)
        {
            return new PaginatedList<TDestination>
            {
                PageNumber = response.PageNumber,
                PageSize = response.PageSize,
                TotalCount = response.TotalCount,
                Collection = response.Collection
                                     .Select(i => mapper.Map<TDestination>(i))
                                     .ToList()
            };
        }
    }

    public class PaginatedList<T> : PaginatedRequestBaseList
    {
        public int TotalPages
        {
            get
            {
                var totalPages = TotalCount / PageSize;

                if (TotalCount % PageSize > 0)
                    totalPages++;

                return totalPages;
            }
        }
        public int TotalCount { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
        public ICollection<T>? Collection { get; set; }
        public static async Task<PaginatedList<T>> CreateInstanceAsync(int pageNumber, int pageSize, IQueryable<T> queryable)
        {
            var paginatedList = new PaginatedList<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = await queryable.CountAsync(),
            };

            var queryablePaginated = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            paginatedList.Collection = await queryablePaginated.ToListAsync();

            return paginatedList;
        }

        public PaginatedList<TDestination> Map<TDestination>(IMapper mapper)
        {
            return new PaginatedList<TDestination>
            {
                PageNumber = this.PageNumber,
                PageSize = this.PageSize,
                TotalCount = this.TotalCount,
                Collection = this.Collection.Select(i => mapper.Map<TDestination>(i)).ToList()
            };
        }
    }
}
