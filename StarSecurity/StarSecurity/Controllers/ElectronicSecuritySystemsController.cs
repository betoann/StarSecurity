using Microsoft.AspNetCore.Mvc;

namespace StarSecurity.Controllers
{
    public class ElectronicSecuritySystemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
