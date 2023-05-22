using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly StarSecurityContext _context;

        public RoleController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListRole()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.Roles.ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> AddRole()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(Role role)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            try
            {
                if (RoleNameExists(role.Name))
                {
                    ViewBag.error = "Role name exists";
                    return View(role);
                }
                if (RoleCodeExists(role.RoleCode))
                {
                    ViewBag.error = "Role Code exists";
                    return View(role);
                }

                role.CreateBy = email;
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return Redirect(nameof(ListRole));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }


            return View(role);
        }

        public async Task<IActionResult> EditRole(string code)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (code == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleCode == code);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string code, Role role)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            try
            {
                role.UpdateBy = email;
                role.UpdatedDate = DateTime.Now;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListRole));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(role);
        }

        public async Task<IActionResult> DeleteRole(string code)
        {
            if (_context.Roles == null)
            {
                return Problem("Role is null");
            }
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleCode == code);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListRole));
        }

        private bool RoleNameExists(string name)
        {
            return _context.Roles.Any(e => e.Name == name);
        }

        private bool RoleCodeExists(string rolecode)
        {
            return _context.Roles.Any(e => e.RoleCode == rolecode);
        }

        private bool CheckRoleAdmin(string email)
        {
            var empl = _context.Employees.SingleOrDefault(e => e.Email == email);
            if (empl.RoleCode == "RoleAdmin")
            {
                return true;
            }
            return false;
        }
    }
}
