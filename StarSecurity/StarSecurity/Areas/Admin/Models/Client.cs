using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api_StarSecurity.Models
{
    public partial class Client
    {
        public Client()
        {
            Tasks = new HashSet<Task>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; } = 0;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
