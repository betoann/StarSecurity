using System.Security.Claims;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Common;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly StarSecurityContext _context;

        public HomeAdminController(StarSecurityContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var data = _context.Accounts.FirstOrDefault(m => m.Username == username);

            if (data == null)
            {
                ViewBag.error = "Username/password incorrect";
                return View();
            };

            var isValid = username.ValidPassword(data.Salt, password, data.Password);
            if (!isValid)
            {
                ViewBag.error = "Username/password incorrect";
                return View();
            };

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == data.EmployeeId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == employee.RoleId);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, employee.Email)
            };
                
            // create identity
            ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

            // create principal
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            // sign-in
            await HttpContext.SignInAsync(
                    scheme: "SecurityScheme",
                    principal: principal,
                    properties: new AuthenticationProperties
                    {
                        IsPersistent = true,
                        //ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
                    });

            return Redirect("Index");

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            scheme: "SecurityScheme");

            return Redirect("Login");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.CountEmployee = await _context.Employees.CountAsync();
            ViewBag.CountClient = await _context.Clients.CountAsync();
            ViewBag.CountTask = await _context.Tasks.CountAsync(t => t.Status == 1);
            

            return View();
        }
    }
}
