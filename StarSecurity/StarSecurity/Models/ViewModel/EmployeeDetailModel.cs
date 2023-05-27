using StarSecurity.Entites;

namespace StarSecurity.Models.ViewModel
{
    public class EmployeeDetailModel
    {
        public Employee employee { get; set; }
        public Department department { get; set; }
        public List<RegisterService>? client { get; set; }
    }
}
