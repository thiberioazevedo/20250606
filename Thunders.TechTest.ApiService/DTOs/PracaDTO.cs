namespace Thunders.TechTest.ApiService.DTOs
{
    public class PracaDTO : BaseDTO
    {
        public string Nome { get; set; }
        //public Guid CidadeId { get; set; }
        public virtual CidadeDTO Cidade { get; set; }

    }
}
