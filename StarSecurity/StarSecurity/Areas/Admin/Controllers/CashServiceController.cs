using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CashServiceController : Controller
    {
        private readonly StarSecurityContext _context;

        public CashServiceController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListCashService(string name)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.CashServices.Where(c => string.IsNullOrEmpty(name) || c.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailCashService(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.CashServices == null)
            {
                return NotFound();
            }
            var res = await _context.CashServices
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> EditCashService(long id)
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

            if (id == null || _context.CashServices == null)
            {
                return NotFound();
            }

            var cashService = await _context.CashServices.FindAsync(id);
            if (cashService == null)
            {
                return NotFound();
            }

            return View(cashService);
        }

        [HttpPost]
        public async Task<IActionResult> EditCashService(long id, CashService cashService)
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

            if (id != cashService.Id)
            {
                return NotFound();
            }

            try
            {
                cashService.UpdateBy = email;
                cashService.UpdatedDate = DateTime.Now;
                _context.CashServices.Update(cashService);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListCashService));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(cashService);
        }

        public async Task<IActionResult> DeleteCashService(long id)
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

            if (_context.CashServices == null)
            {
                return Problem("CashService is null");
            }
            var CashService = await _context.CashServices.FindAsync(id);
            if (CashService != null)
            {
                _context.CashServices.Remove(CashService);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListCashService));
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
