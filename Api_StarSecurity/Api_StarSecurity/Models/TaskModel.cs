namespace Api_StarSecurity.Models
{
    public class TaskModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long ClientId { get; set; }
        public long ServiceId { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; } = 0;
    }
}
