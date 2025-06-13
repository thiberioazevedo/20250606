namespace Thunders.TechTest.ApiService.DTOs
{
    public class FaturamentoHoraCidadeReportDTO : BaseReportDTO
    {
        //public Guid CidadeId { get; set; }
        public CidadeDTO Cidade { get; set; }
        public DateTime Data { get; set; }
        public short Hora { get; set; }
    }
}
