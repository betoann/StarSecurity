
using Microsoft.AspNetCore.Mvc;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {

        public IActionResult ListRole()
        {
            return View();
        }

        public IActionResult AddRole()
        {
            return View();
        }

        public IActionResult EditRole()
        {
            return View();
        }

        public IActionResult DeleteRole()
        {
            return View();
        }
    }
}
