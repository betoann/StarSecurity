namespace StarSecurity.Models
{
    public class ResetPasswordModel
    {
        public long Id { get; set; }
        public string Password { get; set; }
    }

    public class UpdatePasswordModel
    {
        public long Id { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
