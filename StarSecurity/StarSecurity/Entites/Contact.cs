using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Contact
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone cannot be blank")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Message cannot be blank")]
        public string Message { get; set; }
        public int? Status { get; set; } = 2;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
