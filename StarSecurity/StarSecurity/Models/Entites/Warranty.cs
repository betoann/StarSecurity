using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Warranty
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
        [Required(ErrorMessage = "Product cannot be blank")]
        public long ProductId { get; set; }
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "TimeWarranty cannot be blank")]
        public DateTime TimeWarranty { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Product? Product { get; set; }
    }
}
