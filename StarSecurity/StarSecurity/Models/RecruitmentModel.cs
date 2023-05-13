namespace StarSecurity.Models
{
    public class RecruitmentModel
    {
        public long Id { get; set; }
        public string? OnPosition { get; set; }
        public long Count { get; set; }
        public string? Description { get; set; }
        public long? ServiceId { get; set; }
        public int? Status { get; set; }
    }
}
