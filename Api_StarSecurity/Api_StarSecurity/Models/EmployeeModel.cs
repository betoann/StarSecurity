namespace Api_StarSecurity.Models
{
    public class EmployeeModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? EduQualifi { get; set; }
        public string EmployeeCode { get; set; }
        public long ServiceId { get; set; }
        public long RoleId { get; set; }
        public string? Grade { get; set; }
        public string? Achievements { get; set; }
        public int Status { get; set; } = 1;
    }
}
