using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.DTOs
{
    public class VeiculoDTO : BaseDTO
    {
        public string Placa { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }
    }
}
