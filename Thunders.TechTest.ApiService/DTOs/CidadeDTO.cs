namespace Thunders.TechTest.ApiService.DTOs
{
    public class CidadeDTO : BaseDTO
    {
        public string Nome { get; set; }
        //public Guid EstadoId { get; set; }
        public virtual EstadoDTO Estado { get; set; }
    }
}
