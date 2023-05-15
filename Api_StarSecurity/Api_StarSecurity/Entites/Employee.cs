using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api_StarSecurity.Entites
{
    public partial class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
            Tasks = new HashSet<Task>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? EduQualifi { get; set; }
        public string EmployeeCode { get; set; }
        public long ServiceId { get; set; }
        public long RoleId { get; set; }
        public string? Grade { get; set; }
        public string? Achievements { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Role? Role { get; set; }
        public virtual Service? Service { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
