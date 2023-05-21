using System.Web.Helpers;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddService(Service service, IFormFile fileImage)
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
                if (ServiceExists(service.Name))
                {
                    ViewBag.error = "Service exists";
                    return View(service);
                }
                if (service.Description == null)
                {
                    return View(service);
                }

                service.CreateBy = email;
                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                int id = int.Parse(_context.Services.ToList().Last().Id.ToString());

                Service sev = _context.Services.FirstOrDefault(s => s.Id == id);
                sev.Image = UploadImg(fileImage);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListService));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return View(service);
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

            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(long id, Service service, IFormFile fileImage)
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

            if (id != service.Id)
            {
                return NotFound();
            }
            
            try
            {
                if (service.Description == null)
                {
                    return View(service);
                }
                if (fileImage != null)
                {
                    service.Image = UploadImg(fileImage);
                }

                service.UpdateBy = email;
                service.UpdatedDate = DateTime.Now;
                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListService));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return View(service);
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
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListService));
        }

        private bool ServiceExists(string name)
        {
            return _context.Roles.Any(e => e.Name == name);
        }

        private string UploadImg(IFormFile file)
        {
            try
            {
                string FileName = file.FileName;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/service/", FileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return FileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckRoleAdmin(string email)
        {
            var empl = _context.Employees.SingleOrDefault(e => e.Email == email);
            if(empl.RoleCode == "RoleAdmin")
            {
                return true;
            }
            return false;
        }
    }
}
