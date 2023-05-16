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
            Tasks = new HashSet<Task>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string Description { get; set; } = null!;
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Recruitment> Recruitments { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
