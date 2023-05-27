using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

            HttpContext.Response.Cookies.Append("email", employee.Email);

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

            HttpContext.Response.Cookies.Delete("email");

            return Redirect(nameof(Login));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var cEmpl = await _context.Employees.CountAsync();
            var cClient = await _context.RegisterServices.CountAsync();
            var cCash = await _context.CashServices.CountAsync();
            var cCandidate = await _context.Candidates.CountAsync();


            ViewBag.CountEmployee = (cEmpl > 0) ? cEmpl : 0;
            ViewBag.CountClient = (cClient > 0) ? cClient : 0;
            ViewBag.CountCashService = (cCash > 0) ? cCash : 0;
            ViewBag.CountCandidate = (cCandidate > 0) ? cCandidate : 0;

            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> PageError()
        {

            return View();
        }
    }
}
