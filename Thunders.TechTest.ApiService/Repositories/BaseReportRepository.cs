using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Thunders.TechTest.ApiService.Interfaces;
using Thunders.TechTest.ApiService.Pagination;
using Thunders.TechTest.ApiService.Models;
using System.Collections.ObjectModel;

namespace Thunders.TechTest.ApiService.Repositories
{
    public abstract class BaseReportRepository<T> : Repository<T>, IBaseReportRepository<T> where T : BaseReport
    {
        public BaseReportRepository(DbContext context, IEntityCacheProvider entityCacheProvider) : base(context, entityCacheProvider) { }
        public abstract IQueryable<PedagioUtilizacao> GetPedagioUtilizacaoQuery(T baseReport, CancellationToken cancellationToken);
        public IQueryable<T> GetReportsProcessar(CancellationToken cancellationToken)
        {
            return GetListQuery(i => i.Processar, null, cancellationToken);
        }
        public async Task<T> SetReportProcessadoAsync(T baseReport, CancellationToken cancellationToken)
        {
            baseReport.InicioProcessamento = DateTime.Now;

            var countSum = GetPedagioUtilizacaoQuery(baseReport, cancellationToken).GroupBy(i => 1)
                                                                                   .Select(g => new
                                                                                   {
                                                                                       Quantidade = g.Count(),
                                                                                       ValorTotal = g.Sum(x => x.ValorPago)
                                                                                   })
                                                                                   .ToList()
                                                                                   .FirstOrDefault();

            baseReport.ValorTotal = countSum.ValorTotal;
            baseReport.Quantidade = countSum.Quantidade;
            baseReport.Processar = false;
            baseReport.FimProcessamento = DateTime.Now;

            return baseReport;
        }
    }
}
