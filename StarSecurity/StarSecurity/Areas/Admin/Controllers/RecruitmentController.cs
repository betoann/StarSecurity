using Microsoft.AspNetCore.Mvc;


namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecruitmentController : Controller
    {

        public IActionResult ListRecruitment()
        {
            return View();
        }

        public IActionResult DetailRecruitment()
        {
            return View();
        }

        public IActionResult AddRecruitment()
        {
            return View();
        }

        public IActionResult EditRecruitment()
        {
            return View();
        }

        public IActionResult DeleteRecruitment()
        {
            return View();
        }
    }
}
