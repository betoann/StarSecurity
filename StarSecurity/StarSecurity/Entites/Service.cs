using System;
using System.Collections.Generic;

namespace Api_StarSecurity.Entites
{
    public partial class Service
    {
        public Service()
        {
            Employees = new HashSet<Employee>();
            Recruitments = new HashSet<Recruitment>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Recruitment> Recruitments { get; set; }
    }
}
