namespace Thunders.TechTest.ApiService.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        
        public long Sequencial { get; set; }
    }
}