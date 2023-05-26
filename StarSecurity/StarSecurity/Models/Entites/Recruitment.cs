using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Recruitment
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Title cannot be blank")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vacancies cannot be blank")]
        public string Vacancies { get; set; }

        [Required(ErrorMessage = "Count cannot be blank")]
        [Range(1,1000, ErrorMessage = "Minimum 1 and maximum 1000")]
        public long Count { get; set; }
        [Required(ErrorMessage = "TimeStart cannot be blank")]
        [DataType(DataType.Date, ErrorMessage = "Format request dd/mm/yyyy")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime TimeStart { get; set; }
        [Required(ErrorMessage = "TimeEnd cannot be blank")]
        [DataType(DataType.Date, ErrorMessage = "Format request dd/mm/yyyy")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime TimeEnd { get; set; }

        public string? Location { get; set; }
        public string? Salary { get; set; }
        public string? Request { get; set; }
        public string? Benefit { get; set; }

        [Required(ErrorMessage = "Please describe in detail")]
        public string Description { get; set; }

        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
