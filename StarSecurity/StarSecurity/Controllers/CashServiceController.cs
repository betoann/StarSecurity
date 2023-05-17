using Microsoft.AspNetCore.Mvc;

namespace StarSecurity.Controllers
{
    public class CashServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
