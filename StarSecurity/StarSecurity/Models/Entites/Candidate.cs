using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Candidate
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Message cannot be blank")]
        public string Message { get; set; }
        [Required(ErrorMessage = "FileCv cannot be blank")]
        public string FileCv { get; set; }
        [Required(ErrorMessage = "RecruitmentId cannot be blank")]
        public long RecruitmentId { get; set; }
        public int? Status { get; set; } = 1;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public virtual Recruitment Recruitment { get; set; } = null!;
    }
}
