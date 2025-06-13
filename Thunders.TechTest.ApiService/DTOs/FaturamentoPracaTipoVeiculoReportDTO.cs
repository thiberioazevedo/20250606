using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.DTOs
{
    public class FaturamentoPracaTipoVeiculoReportDTO : BaseReportDTO
    {
        //public Guid PracaId { get; set; }
        public PracaDTO Praca { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }
    }
}
