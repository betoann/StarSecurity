namespace Api_StarSecurity.Models
{
    public class ContactModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public int? Status { get; set; } = 0;
    }
}
