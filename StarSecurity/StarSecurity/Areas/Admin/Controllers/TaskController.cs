using Microsoft.AspNetCore.Mvc;


namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaskController : Controller
    {

        public IActionResult ListTask()
        {
            return View();
        }

        public IActionResult DetailTask()
        {
            return View();
        }

        public IActionResult AddTask()
        {
            return View();
        }

        public IActionResult EditTask()
        {
            return View();
        }

        public IActionResult DeleteTask()
        {
            return View();
        }
    }
}
