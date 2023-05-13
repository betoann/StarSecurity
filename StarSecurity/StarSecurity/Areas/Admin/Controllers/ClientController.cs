using Microsoft.AspNetCore.Mvc;


namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClientController : Controller
    {

        public IActionResult ListClient()
        {
            return View();
        }

        public IActionResult DetailClient()
        {
            return View();
        }

        public IActionResult AddClient()
        {
            return View();
        }

        public IActionResult EditClient()
        {
            return View();
        }

        public IActionResult DeleteClient()
        {
            return View();
        }
    }
}
