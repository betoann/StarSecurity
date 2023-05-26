using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Department
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Summary cannot be blank")]
        public string Summary { get; set; }
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; } = null!;
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
