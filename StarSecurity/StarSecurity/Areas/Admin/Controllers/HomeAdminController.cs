using Microsoft.AspNetCore.Mvc;


namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if(username == "admin" && password == "1")
            {
                return Redirect("Index");
            }
            else
            {
                ViewData["error"] = "Username/password incorrect";
                return View();
            }
        }

        public IActionResult Logout()
        {
            return Redirect("Login");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
