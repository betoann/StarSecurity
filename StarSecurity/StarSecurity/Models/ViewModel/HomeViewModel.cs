using StarSecurity.Entites;

namespace StarSecurity.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Department> departments { get; set; }
        public List<Partner> partners { get; set; }
        public List<Service> services { get; set; }
        public List<Product> products { get; set; }
    }
}
