namespace Thunders.TechTest.ApiService.DTOs
{
    public class PedagioUtilizacaoDTO : BaseDTO
    {
        public DateTime DataHoraUtilizacao { get; set; }
        public Guid PracaId { get; set; }
        public Guid VeiculoId { get; set; }
        public decimal ValorPago { get; set; }
        public virtual PracaDTO Praca { get; set; }
        public virtual VeiculoDTO Veiculo { get; set; }
    }
}
