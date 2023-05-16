using System;
using System.Collections.Generic;

namespace Api_StarSecurity.Entites
{
    public partial class Contact
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Message { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
