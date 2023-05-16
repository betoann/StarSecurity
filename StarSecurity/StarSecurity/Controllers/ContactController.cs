using Microsoft.AspNetCore.Mvc;

namespace StarSecurity.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
