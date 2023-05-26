using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly StarSecurityContext _context;

        public ServiceController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListService()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.Services.ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailService(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.Services == null)
            {
                return NotFound();
            }
            var res = await _context.Services
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddService()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            var department = await _context.Departments.ToListAsync();
            ViewBag.Department = new SelectList(department, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddService(Service Service)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            var department = await _context.Departments.ToListAsync();
            ViewBag.Department = new SelectList(department, "Id", "Name");

            try
            {
                if (ServiceExists(Service.Name))
                {
                    ViewBag.error = "Service exists";
                    return View(Service);
                }
                if (Service.Description == null)
                {
                    return View(Service);
                }

                Service.CreateBy = email;
                _context.Services.Add(Service);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListService));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Service);
        }

        public async Task<IActionResult> EditService(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            var department = await _context.Departments.ToListAsync();
            ViewBag.Department = new SelectList(department, "Id", "Name");

            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var Service = await _context.Services.FindAsync(id);
            if (Service == null)
            {
                return NotFound();
            }
            return View(Service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(long id, Service Service)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            if (id != Service.Id)
            {
                return NotFound();
            }

            var department = await _context.Departments.ToListAsync();
            ViewBag.Department = new SelectList(department, "Id", "Name");

            try
            {
                if (Service.Description == null)
                {
                    return View(Service);
                }

                Service.UpdateBy = email;
                Service.UpdatedDate = DateTime.Now;
                _context.Services.Update(Service);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListService));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Service);
        }

        public async Task<IActionResult> DeleteService(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            if (_context.Services == null)
            {
                return Problem("Service is null");
            }
            var Service = await _context.Services.FindAsync(id);
            if (Service != null)
            {
                _context.Services.Remove(Service);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListService));
        }

        private bool ServiceExists(string name)
        {
            return _context.Services.Any(e => e.Name == name);
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
