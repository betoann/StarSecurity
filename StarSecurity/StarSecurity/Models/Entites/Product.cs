using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarSecurity.Entites
{
    public partial class Product
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Price cannot be blank")]
        public decimal Price { get; set; }
        public string? Feature { get; set; }
        public string? Description { get; set; }
        public int? Warranty { get; set; }
        [Required(ErrorMessage = "Partner cannot be blank")]
        public long PartnerId { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Partner? Partner { get; set; }
        public virtual ICollection<RegisterService> RegisterServices { get; set; }
        public virtual ICollection<Warranty> Warranties { get; set; }
    }
}
