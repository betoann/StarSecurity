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
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Department cannot be blank")]
        public long DepartmentId { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<RegisterService> RegisterServices { get; set; }
    }
}
