namespace Thunders.TechTest.ApiService.Models
{
    public class FaturamentoPracaMesReport : BaseReport
    {
        public Guid PracaId { get; set; }
        public short Ano { get; set; }
        public short Mes { get; set; }
        public virtual Praca Praca { get; set; }

    }
}