using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Client
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        public string? Gender { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Format request dd/mm/yyyy")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dob { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone cannot be blank")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please describe in detail")]
        public string Description { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
