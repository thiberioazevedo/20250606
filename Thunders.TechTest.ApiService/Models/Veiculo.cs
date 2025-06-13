namespace Thunders.TechTest.ApiService.Models
{
    public class Veiculo : BaseEntity
    {
        public string Placa { get; set; }
        public ETipoVeiculo TipoVeiculo { get; set; }
        public virtual ICollection<PedagioUtilizacao> PedagioUtilizacaoColletction { get; set; }

    }
}