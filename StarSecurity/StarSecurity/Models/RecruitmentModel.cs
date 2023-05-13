namespace Api_StarSecurity.Models
{
    public class RecruitmentModel
    {
        public long Id { get; set; }
        public string Vacancies { get; set; }
        public long Count { get; set; }
        public long ServiceId { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
    }
}
