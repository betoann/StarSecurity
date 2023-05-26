using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Account
    {
        public long Id { get; set; }
        public string? Salt { get; set; }
        [Required(ErrorMessage = "Username cannot be blank")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        [RegularExpression(@"^[A-Za-z 0-9]*$", ErrorMessage = "Cannot use special character")]
        [MinLength(6, ErrorMessage = "Password needs at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select staff")]
        public long EmployeeId { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
