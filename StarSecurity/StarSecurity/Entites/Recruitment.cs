using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Recruitment
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vacancies cannot be blank")]
        public string Vacancies { get; set; }
        [Required(ErrorMessage = "Count cannot be blank")]
        [Range(1,1000, ErrorMessage = "Minimum 1 and maximum 1000")]
        public long Count { get; set; }
        [Required(ErrorMessage = "Department cannot be blank")]
        public long ServiceId { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Summary cannot be blank")]
        public string Summary { get; set; }
        [Required(ErrorMessage = "Please describe in detail")]
        public string Description { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Service? Service { get; set; }
    }
}
