using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public class EmployeeDetail
    {
        public Employee Employee { get; set; }
    }
    public partial class Employee
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Dob { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? EduQualifi { get; set; }
        [Required(ErrorMessage = "EmployeeCode cannot be blank")]
        public string? EmployeeCode { get; set; }
        [Required(ErrorMessage = "Department cannot be blank")]
        public long DepartmentId { get; set; }
        [Required(ErrorMessage = "Role cannot be blank")]
        public string RoleCode { get; set; }
        public string? Grade { get; set; }
        public string? Achievements { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Role? RoleCodeNavigation { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
