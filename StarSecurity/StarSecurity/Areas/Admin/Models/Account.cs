using System;
using System.Collections.Generic;

namespace Api_StarSecurity.Models
{
    public partial class Account
    {
        public long Id { get; set; }
        public string? Salt { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long EmployeeId { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
