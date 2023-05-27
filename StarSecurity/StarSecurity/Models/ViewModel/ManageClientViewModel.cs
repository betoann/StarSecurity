using StarSecurity.Entites;

namespace StarSecurity.Models.ViewModel
{
    public class ManageClientViewModel
    {
        public RegisterService registerService { get; set; }
        public Service? service { get; set; }
        public Product? product { get; set; }
        public Employee? employee { get; set; }
    }
}
