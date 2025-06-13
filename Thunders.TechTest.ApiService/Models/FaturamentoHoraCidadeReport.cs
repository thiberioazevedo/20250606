namespace Thunders.TechTest.ApiService.Models
{
    public class FaturamentoHoraCidadeReport : BaseReport
    {
        public Guid CidadeId { get; set; }
        public DateTime Data { get; set; }
        public short Hora { get; set; }
        public virtual Cidade Cidade { get; set; }
    }
}