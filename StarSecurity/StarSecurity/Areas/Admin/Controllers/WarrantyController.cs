using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class WarrantyController : Controller
    {
        private readonly StarSecurityContext _context;

        public WarrantyController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListWarranty(string name)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.Warranties.Where(w => string.IsNullOrEmpty(name) || w.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailWarranty(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.Warranties == null)
            {
                return NotFound();
            }
            var res = await _context.Warranties
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddWarranty()
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

            var product = await _context.Products.ToListAsync();
            ViewBag.Product = new SelectList(product, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWarranty(Warranty Warranty)
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

            var product = await _context.Products.ToListAsync();
            ViewBag.Product = new SelectList(product, "Id", "Name");

            try
            {
                Warranty.CreateBy = email;
                _context.Warranties.Add(Warranty);
                await _context.SaveChangesAsync();

                var w = _context.Warranties.ToList().Last();
                Product prd = _context.Products.FirstOrDefault(s => s.Id == w.ProductId);
                w.TimeWarranty = DateTime.Today.AddMonths((int)prd.Warranty);
                w.Price = prd.Price;
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListWarranty));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Warranty);
        }

        public async Task<IActionResult> EditWarranty(long id)
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

            var product = await _context.Products.ToListAsync();
            ViewBag.Product = new SelectList(product, "Id", "Name");

            if (id == null || _context.Warranties == null)
            {
                return NotFound();
            }

            var Warranty = await _context.Warranties.FindAsync(id);
            if (Warranty == null)
            {
                return NotFound();
            }
            return View(Warranty);
        }

        [HttpPost]
        public async Task<IActionResult> EditWarranty(long id, Warranty Warranty)
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

            if (id != Warranty.Id)
            {
                return NotFound();
            }

            var product = await _context.Products.ToListAsync();
            ViewBag.Product = new SelectList(product, "Id", "Name");

            try
            {
                Warranty.UpdateBy = email;
                Warranty.UpdatedDate = DateTime.Now;
                _context.Warranties.Update(Warranty);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListWarranty));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Warranty);
        }

        public async Task<IActionResult> DeleteWarranty(long id)
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

            if (_context.Warranties == null)
            {
                return Problem("Warranty is null");
            }
            var Warranty = await _context.Warranties.FindAsync(id);
            if (Warranty != null)
            {
                _context.Warranties.Remove(Warranty);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListWarranty));
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
