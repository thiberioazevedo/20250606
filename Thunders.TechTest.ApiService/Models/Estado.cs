namespace Thunders.TechTest.ApiService.Models
{
    public class Estado : BaseEntity
    {
        public string UF { get; set; }
        public string Nome { get; set; }
        public ICollection<Cidade> CidadeCollection { get; set; }
    }
}