using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Task
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Client cannot be blank")]
        public long EmployeeId { get; set; }
        [Required(ErrorMessage = "Employee cannot be blank")]
        public long ClientId { get; set; }
        [Required(ErrorMessage = "Service cannot be blank")]
        public long ServiceId { get; set; }
        [Required(ErrorMessage = "Please describe in detail")]
        public string Description { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public virtual Service? Service { get; set; }
    }
}
