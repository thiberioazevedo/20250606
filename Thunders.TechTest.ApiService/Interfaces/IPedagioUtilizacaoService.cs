using AutoMapper;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.Pagination;
using Thunders.TechTest.ApiService.Repositories;

namespace Thunders.TechTest.ApiService.Interfaces
{
    public interface IPedagioUtilizacaoService
    {
        Task<PedagioUtilizacaoDTO> CreatePedagioUtilizacaoAsync(PedagioUtilizacaoCreateRequestDTO pedagioUtilizacaoCreateRequestDTO, CancellationToken cancellationToken);
        Task<IList<PedagioUtilizacaoDTO>> CreatePedagioUtilizacaoRangeAsync(IList<PedagioUtilizacaoCreateRequestDTO> pedagioUtilizacaoCreateRequestDTOList, CancellationToken cancellationToken);
        Task<PaginatedList<PedagioUtilizacaoDTO>> GetPaginatedList(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<ProcessamentoRelatoriosDTO> ProcessamentoRelatoriosAsync(CancellationToken cancellationToken);
    }
}
