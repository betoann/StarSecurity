using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class RegisterService
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone cannot be blank")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }
        public long? ServiceId { get; set; }
        public long? ProductId { get; set; }
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }
        public long? EmployeeId { get; set; }
        public int? Status { get; set; } = 0;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual Employee? Employee { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Service? Service { get; set; }
    }
}
