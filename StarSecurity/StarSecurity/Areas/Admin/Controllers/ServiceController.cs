using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {

        public IActionResult ListService()
        {
            return View();
        }

        public IActionResult DetailService()
        {
            return View();
        }

        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult AddService(string description)
        {
            ViewBag.ds = description;
            return View();
        }

        public IActionResult EditService()
        {
            return View();
        }

        public IActionResult DeleteService()
        {
            return View();
        }
    }
}
