namespace Thunders.TechTest.ApiService.Models
{
    public class Praca : BaseEntity
    {
        public string Nome { get; set; }
        public Guid CidadeId { get; set; }
        public virtual Cidade Cidade { get; set; }
        public virtual ICollection<PedagioUtilizacao> PedagioUtilizacaoColletction { get; set; }
        public virtual ICollection<FaturamentoPracaTipoVeiculoReport> FaturamentoPracaTipoVeiculoReportCollection { get; set; }
        public virtual ICollection<FaturamentoPracaMesReport> FaturamentoPracaMesReportCollection { get; set; }
    }
}