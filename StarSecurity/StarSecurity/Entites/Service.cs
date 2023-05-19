using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Service
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
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

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Recruitment> Recruitments { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
