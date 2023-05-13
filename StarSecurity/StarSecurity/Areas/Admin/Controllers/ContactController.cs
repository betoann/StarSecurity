using Microsoft.AspNetCore.Mvc;


namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {

        public IActionResult ListContact()
        {
            return View();
        }

        public IActionResult DetailContact()
        {
            return View();
        }

        public IActionResult DeleteContact()
        {
            return View();
        }
    }
}
