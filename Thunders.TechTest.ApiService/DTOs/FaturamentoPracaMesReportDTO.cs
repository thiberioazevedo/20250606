namespace Thunders.TechTest.ApiService.DTOs
{
    public class FaturamentoPracaMesReportDTO : BaseReportDTO
    {
        //public Guid PracaId { get; set; }
        public PracaDTO Praca { get; set; }
        public short Ano { get; set; }
        public short Mes { get; set; }

    }
}
