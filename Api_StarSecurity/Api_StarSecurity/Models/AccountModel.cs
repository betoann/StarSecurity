namespace Api_StarSecurity.Models
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public long? EmployeeId { get; set; }
    }
}
