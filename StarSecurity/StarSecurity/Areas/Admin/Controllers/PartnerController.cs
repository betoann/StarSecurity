using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PartnerController : Controller
    {
        private readonly StarSecurityContext _context;

        public PartnerController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListPartner()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.Partners.ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailPartner(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.Partners == null)
            {
                return NotFound();
            }
            var res = await _context.Partners
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddPartner()
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPartner(Partner Partner, IFormFile fileImage)
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

            try
            {
                if (PartnerExists(Partner.Name))
                {
                    ViewBag.error = "Partner exists";
                    return View(Partner);
                }

                Partner.CreateBy = email;
                _context.Partners.Add(Partner);
                await _context.SaveChangesAsync();

                if(fileImage != null)
                {
                    int id = int.Parse(_context.Partners.ToList().Last().Id.ToString());
                    Partner pt = _context.Partners.FirstOrDefault(s => s.Id == id);
                    pt.Image = UploadImg(fileImage);
                    await _context.SaveChangesAsync();
                }

                return Redirect(nameof(ListPartner));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Partner);
        }

        public async Task<IActionResult> EditPartner(long id)
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

            if (id == null || _context.Partners == null)
            {
                return NotFound();
            }

            var Partner = await _context.Partners.FindAsync(id);
            if (Partner == null)
            {
                return NotFound();
            }
            return View(Partner);
        }

        [HttpPost]
        public async Task<IActionResult> EditPartner(long id, Partner Partner, IFormFile fileImage)
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

            if (id != Partner.Id)
            {
                return NotFound();
            }

            try
            {

                Partner.UpdateBy = email;
                Partner.UpdatedDate = DateTime.Now;
                if (fileImage != null)
                {
                    Partner.Image = UploadImg(fileImage);
                }

                Partner.UpdateBy = email;
                Partner.UpdatedDate = DateTime.Now;
                _context.Partners.Update(Partner);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListPartner));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Partner);
        }

        public async Task<IActionResult> DeletePartner(long id)
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

            if (_context.Partners == null)
            {
                return Problem("Partner is null");
            }
            var Partner = await _context.Partners.FindAsync(id);
            if (Partner != null)
            {
                _context.Partners.Remove(Partner);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListPartner));
        }

        private bool PartnerExists(string name)
        {
            return _context.Partners.Any(e => e.Name == name);
        }

        private bool PartnerEmailExists(string email)
        {
            return _context.Partners.Any(e => e.Email == email);
        }

        private string UploadImg(IFormFile file)
        {
            try
            {
                string FileName = file.FileName;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/partner/", uniqueFileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
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
