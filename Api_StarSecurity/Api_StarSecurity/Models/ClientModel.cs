namespace Api_StarSecurity.Models
{
    public class ClientModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
    }
}
