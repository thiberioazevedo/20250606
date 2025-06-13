namespace Thunders.TechTest.ApiService.Models
{
    public class BaseReport : BaseEntity
    {
        public DateTime? DataHoraSolicitacao { get; set; }
        public DateTime? InicioProcessamento { get; set; }
        public DateTime? FimProcessamento { get; set; }
        public bool Processar { get; set; }
        public long Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }
}