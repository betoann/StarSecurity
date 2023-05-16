using System;
using System.Collections.Generic;

namespace Api_StarSecurity.Entites
{
    public partial class Task
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long ClientId { get; set; }
        public long? ServiceId { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; } = 0;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public virtual Service? Service { get; set; }
    }
}
