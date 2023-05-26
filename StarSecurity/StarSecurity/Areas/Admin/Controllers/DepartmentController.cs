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
    public class DepartmentController : Controller
    {
        private readonly StarSecurityContext _context;

        public DepartmentController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListDepartment()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.Departments.ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailDepartment(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }
            var res = await _context.Departments
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddDepartment()
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
        public async Task<IActionResult> AddDepartment(Department Department, IFormFile fileImage)
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
                if (DepartmentExists(Department.Name))
                {
                    ViewBag.error = "Department exists";
                    return View(Department);
                }
                if (Department.Description == null)
                {
                    return View(Department);
                }

                Department.CreateBy = email;
                _context.Departments.Add(Department);
                await _context.SaveChangesAsync();

                if (fileImage != null)
                {
                    int id = int.Parse(_context.Departments.ToList().Last().Id.ToString());
                    Department dpm = _context.Departments.FirstOrDefault(s => s.Id == id);
                    dpm.Image = UploadImg(fileImage);
                    await _context.SaveChangesAsync();
                }

                return Redirect(nameof(ListDepartment));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return View(Department);
        }

        public async Task<IActionResult> EditDepartment(long id)
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

            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var Department = await _context.Departments.FindAsync(id);
            if (Department == null)
            {
                return NotFound();
            }
            return View(Department);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartment(long id, Department Department, IFormFile fileImage)
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

            if (id != Department.Id)
            {
                return NotFound();
            }
            
            try
            {
                if (Department.Description == null)
                {
                    return View(Department);
                }
                if (fileImage != null)
                {
                    Department.Image = UploadImg(fileImage);
                }

                Department.UpdateBy = email;
                Department.UpdatedDate = DateTime.Now;
                _context.Departments.Update(Department);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListDepartment));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return View(Department);
        }

        public async Task<IActionResult> DeleteDepartment(long id)
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

            if (_context.Departments == null)
            {
                return Problem("Department is null");
            }
            var Department = await _context.Departments.FindAsync(id);
            if (Department != null)
            {
                _context.Departments.Remove(Department);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListDepartment));
        }

        private bool DepartmentExists(string name)
        {
            return _context.Departments.Any(e => e.Name == name);
        }

        private string UploadImg(IFormFile file)
        {
            try
            {
                string FileName = file.FileName;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/department/", uniqueFileName);
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
            if(empl.RoleCode == "RoleAdmin")
            {
                return true;
            }
            return false;
        }
    }
}
