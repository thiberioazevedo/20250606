using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.DTOs
{
    public class PedagioUtilizacaoCreateRequestDTO : BaseDTO
    {
        public DateTime DataHoraUtilizacao { get; set; }
        public string Praca { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public decimal ValorPago { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }
        public string Placa { get; set; }
    }
}
