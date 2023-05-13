namespace StarSecurity.Models
{
    public class StaffAssignModel
    {
        public long EmployeeId { get; set; }
        public long ClientId { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
    }
}
