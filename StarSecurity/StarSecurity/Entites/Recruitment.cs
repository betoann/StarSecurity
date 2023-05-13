using System;
using System.Collections.Generic;

namespace Api_StarSecurity.Entites
{
    public partial class Recruitment
    {
        public long Id { get; set; }
        public string Vacancies { get; set; }
        public long Count { get; set; }
        public long ServiceId { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; } = 1;
        public string? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Service? Service { get; set; }
    }
}
