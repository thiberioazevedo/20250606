using AutoMapper;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Infrastructure.Mappings
{
    namespace Thunders.TechTest.Application.Mappings
    {
        public class PedagioUtilizacaoProfile : Profile
        {
            public PedagioUtilizacaoProfile()
            {
                CreateMap<Cidade, CidadeDTO>();
                CreateMap<Estado, EstadoDTO>();
                CreateMap<PedagioUtilizacao, PedagioUtilizacaoDTO>();
                CreateMap<Praca, PracaDTO>();
                CreateMap<Veiculo, VeiculoDTO>();
                CreateMap<FaturamentoHoraCidadeReport, FaturamentoHoraCidadeReportDTO>();
                CreateMap<FaturamentoPracaMesReport, FaturamentoPracaMesReportDTO>();
                CreateMap<FaturamentoPracaTipoVeiculoReport, FaturamentoPracaTipoVeiculoReportDTO>();
                CreateMap<FaturamentoPracaTipoVeiculoReport, FaturamentoPracaTipoVeiculoReportDTO>();
            }
        }
    }
}
