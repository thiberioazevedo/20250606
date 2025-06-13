namespace Thunders.TechTest.ApiService.Models
{
    public class FaturamentoPracaTipoVeiculoReport : BaseReport
    {
        public Guid PracaId { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }
        public virtual Praca Praca { get; set; }
    }
}