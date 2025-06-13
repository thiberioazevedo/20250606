namespace Thunders.TechTest.ApiService.Models
{
    public class PedagioUtilizacao : BaseEntity
    {
        public PedagioUtilizacao()
        {
        }

        public PedagioUtilizacao(DateTime dataHoraUtilizacao, Guid pracaId, Guid veiculoId, decimal valorPago)
        {
            DataHoraUtilizacao = dataHoraUtilizacao;
            PracaId = pracaId;
            VeiculoId = veiculoId;
            ValorPago = valorPago;
        }

        public DateTime DataHoraUtilizacao { get; set; }
        public Guid PracaId { get; set; }
        public Guid VeiculoId { get; set; }
        public decimal ValorPago { get; set; }
        public virtual Praca Praca { get; set; }
        public virtual Veiculo Veiculo { get; set; }
    }
}