namespace Thunders.TechTest.ApiService.Models
{
    public class Cidade : BaseEntity
    {
        public string Nome { get; set; }
        public Guid EstadoId { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ICollection<Praca> PracaCollection { get; set; }
        public virtual ICollection<FaturamentoHoraCidadeReport> FaturamentoHoraCidadeReportCollection { get; set; }
    }
}