using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Partner
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Email { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
