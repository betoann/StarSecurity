using Microsoft.AspNetCore.Mvc;

namespace StarSecurity.Controllers
{
    public class MannedGuardingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
