using Microsoft.AspNetCore.Mvc;

namespace StarSecurity.Controllers
{
    public class OurbusinessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
