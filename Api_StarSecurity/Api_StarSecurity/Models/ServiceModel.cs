namespace Api_StarSecurity.Models
{
    public class ServiceModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string Image { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; } = 1;
    }
}
