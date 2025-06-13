using AutoMapper;
using Thunders.TechTest.ApiService.DTOs;
using Thunders.TechTest.ApiService.Events;
using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Data.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PedagioUtilizacaoCreateRequestDTO, PedagioUtilizacaoCriadoEvent>();
            //CreateMap<PedagioUtilizacao, PedagioUtilizacaoCriadoEvent>();
            CreateMap<PedagioUtilizacao, PedagioUtilizacaoDTO>();
            CreateMap<FaturamentoHoraCidadeReport, FaturamentoHoraCidadeReportDTO>();
            CreateMap<FaturamentoPracaMesReport, FaturamentoPracaMesReportDTO>();
            CreateMap<FaturamentoPracaTipoVeiculoReport, FaturamentoPracaTipoVeiculoReportDTO>();
        }
    }
}
