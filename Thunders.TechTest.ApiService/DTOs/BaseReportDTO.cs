namespace Thunders.TechTest.ApiService.DTOs
{
    public class BaseReportDTO : BaseDTO
    {
        public DateTime? DataHoraSolicitacao { get; set; }
        public DateTime? InicioProcessamento { get; set; }
        public DateTime? FimProcessamento { get; set; }
        //public bool Processar { get; set; }
        public long Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
