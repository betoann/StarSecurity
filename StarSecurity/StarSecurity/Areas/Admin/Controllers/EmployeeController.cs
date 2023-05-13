using Microsoft.AspNetCore.Mvc;


namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {

        public IActionResult ListEmployee()
        {
            return View();
        }

        public IActionResult DetailEmployee()
        {
            return View();
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        public IActionResult EditEmployee()
        {
            return View();
        }

        public IActionResult EditRoleEmpl()
        {
            return View();
        }

        public IActionResult DeleteEmployee()
        {
            return View();
        }
    }
}
