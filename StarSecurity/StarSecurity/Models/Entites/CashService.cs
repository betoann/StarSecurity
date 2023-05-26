using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class CashService
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone cannot be blank")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Amount cannot be blank")]
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public long OrderId { get; set; }
        public long PaymentId { get; set; }
        public long TransactionId { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
