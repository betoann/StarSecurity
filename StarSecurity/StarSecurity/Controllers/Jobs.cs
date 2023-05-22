using Microsoft.AspNetCore.Mvc;

namespace StarSecurity.Controllers
{
    public class Jobs : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
