using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarSecurity.Entites;
using StarSecurity.Models.ViewModel;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ManageClientController : Controller
    {
        private readonly StarSecurityContext _context;

        public ManageClientController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListClient(string name)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.RegisterServices.Where(m => string.IsNullOrEmpty(name) || m.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailClient(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.RegisterServices == null)
            {
                return NotFound();
            }
            var res = await _context.RegisterServices
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }

            var sev = await _context.Services.FirstOrDefaultAsync(s => s.Id == res.ServiceId);
            var prd = await _context.Products.FirstOrDefaultAsync(s => s.Id == res.ProductId);
            var empl = await _context.Employees.FirstOrDefaultAsync(s => s.Id == res.EmployeeId);

            var viewData = new ManageClientViewModel
            {
                registerService = res,
                service = sev,
                product = prd,
                employee = empl,
            };

            return View(viewData);
        }

        public async Task<IActionResult> StaffAssign(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null)
            {
                return NotFound();
            }

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");

            }

            var isStatus = await _context.RegisterServices.FirstOrDefaultAsync(r => r.Id == id);
            if(isStatus.Status == 1)
            {
                return Redirect(nameof(ListClient));
            }

            var empl = _context.Employees.ToList();
            ViewBag.Employee = new SelectList(empl, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StaffAssign(long id, RegisterService registerService)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null)
            {
                return NotFound();
            }

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");

            }

            var isStatus = await _context.RegisterServices.FirstOrDefaultAsync(r => r.Id == id);
            if (isStatus.Status == 1)
            {
                return Redirect(nameof(ListClient));
            }

            var empl = _context.Employees.ToList();
            ViewBag.Employee = new SelectList(empl, "Id", "Name");

            try
            {
                var client = await _context.RegisterServices.FirstOrDefaultAsync(r => r.Id == id);
                client.EmployeeId = registerService.EmployeeId;
                client.UpdateBy = email;
                client.UpdatedDate = DateTime.Now;
                _context.RegisterServices.Update(client);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListClient));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Redirect(nameof(ListClient));

        }

        public async Task<IActionResult> EditStatus(long id)
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

            var isEmployee = await _context.RegisterServices.FirstOrDefaultAsync(r => r.Id == id);
            if (isEmployee.EmployeeId == null)
            {
                return Redirect(nameof(ListClient));
            }

            try
            {
                var client = await _context.RegisterServices.FirstOrDefaultAsync(r => r.Id == id);
                client.Status = 1;
                client.UpdateBy = email;
                client.UpdatedDate = DateTime.Now;
                _context.RegisterServices.Update(client);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListClient));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Redirect(nameof(ListClient));

        }

        public async Task<IActionResult> DeleteClient(long id)
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

            if (_context.RegisterServices == null)
            {
                return Problem("Client is null");
            }
            var client = await _context.RegisterServices.FindAsync(id);
            if (client != null)
            {
                _context.RegisterServices.Remove(client);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListClient));
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
