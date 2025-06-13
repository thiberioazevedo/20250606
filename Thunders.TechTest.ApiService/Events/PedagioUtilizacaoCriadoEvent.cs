using Thunders.TechTest.ApiService.Models;

namespace Thunders.TechTest.ApiService.Events
{
    public class PedagioUtilizacaoCriadoEvent
    {
        //public Guid Id { get; set; }
        public DateTime DataHoraUtilizacao { get; set; }
        public Guid PracaId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid CidadeId { get; set; }
        //public decimal ValorPago { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }
        public string Praca { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        //public string Placa { get; set; }
    }
}