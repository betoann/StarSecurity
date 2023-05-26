using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Areas.Admin.Models
{
    public class UpdatePassWord
    {
        [Required(ErrorMessage = "Username cannot be blank")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        [RegularExpression(@"^[A-Za-z 0-9]*$", ErrorMessage = "Cannot use special character")]
        [MinLength(6, ErrorMessage = "Password needs at least 6 characters")]
        public string PasswordOld { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        [RegularExpression(@"^[A-Za-z 0-9]*$", ErrorMessage = "Cannot use special character")]
        [MinLength(6, ErrorMessage = "Password needs at least 6 characters")]
        public string PasswordNew { get; set; }
    }
}
