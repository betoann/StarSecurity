using System.Security.Principal;
using System.Web.Helpers;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarSecurity.Areas.Admin.Models;
using StarSecurity.Common;
using StarSecurity.Entites;
using StarSecurity.Models.ViewModel;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly StarSecurityContext _context;

        public EmployeeController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListEmployee(int status, long departmentId, string name)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;
            var employee = await _context.Employees.Where(m => (status == 0 || m.Status == status)
                                                    && (departmentId == 0 || m.DepartmentId == departmentId)
                                                    && (string.IsNullOrEmpty(name) || m.Name.ToLower().Contains(name.ToLower()))).ToListAsync();

            var department = await _context.Departments.ToListAsync();
            ViewBag.Department = new SelectList(department, "Id", "Name");

            return View(employee);
        }

        public async Task<IActionResult> DetailEmployee(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }
            var res = await _context.Employees
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == res.DepartmentId);
            var client = await _context.RegisterServices.Where(s => s.EmployeeId == res.Id).ToListAsync();

            var viewData = new EmployeeDetailModel
            {
                employee = res,
                department = department,
                client = client
            };

            return View(viewData);
        }

        public async Task<IActionResult> AddEmployee()
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

            var role = await _context.Roles.ToListAsync();
            ViewBag.Role = new SelectList(role, "RoleCode", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee, IFormFile fileAvatar)
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

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

            var role = await _context.Roles.ToListAsync();
            ViewBag.Role = new SelectList(role, "RoleCode", "Name");

            try
            {
                if (EmailExists(employee.Email))
                {
                    ViewBag.error = "Email exists";
                    return View(employee);
                }

                if (EmployeeCodeExists(employee.EmployeeCode))
                {
                    ViewBag.error = "Email exists";
                    return View(employee);
                }

                employee.CreateBy = email;
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                if (fileAvatar != null)
                {
                    int id = int.Parse(_context.Employees.ToList().Last().Id.ToString());
                    Employee epl = _context.Employees.FirstOrDefault(s => s.Id == id);
                    epl.Avatar = UploadImg(fileAvatar);
                    await _context.SaveChangesAsync();
                }

                return Redirect(nameof(ListEmployee));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(employee);
        }

        public async Task<IActionResult> EditEmployee(long id)
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

            if (emplView.Id == id)
            {
                return Redirect(nameof(ListEmployee));
            }

            var department = await _context.Departments.ToListAsync();
            ViewBag.Department = new SelectList(department, "Id", "Name");

            var role = await _context.Roles.ToListAsync();
            ViewBag.Role = new SelectList(role, "RoleCode", "Name");

            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var empl = await _context.Employees.FindAsync(id);
            if (empl == null)
            {
                return NotFound();
            }

            return View(empl);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(long id,Employee employee, IFormFile fileAvatar)
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

            if (emplView.Id == id)
            {
                return Redirect(nameof(ListEmployee));
            }

            var department = await _context.Departments.ToListAsync();
            ViewBag.Department = new SelectList(department, "Id", "Name");

            var role = await _context.Roles.ToListAsync();
            ViewBag.Role = new SelectList(role, "RoleCode", "Name");

            if (id != employee.Id)
            {
                return NotFound();
            }

            try
            {
                if (fileAvatar != null)
                {
                    employee.Avatar = UploadImg(fileAvatar);
                }
                employee.UpdateBy = email;
                employee.UpdatedDate = DateTime.Now;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListEmployee));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(employee);
        }

        public async Task<IActionResult> AddAcount()
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

            var empl = _context.Employees.ToList();
            ViewBag.Employee = new SelectList(empl, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAcount(Account account)
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

            var empl = _context.Employees.ToList();
            ViewBag.Employee = new SelectList(empl, "Id", "Name");

            try
            {
                if (AccountExists(account.EmployeeId))
                {
                    ViewBag.error = "Employee already has an account";
                    return View(account);
                }
                if (UsernameExists(account.Username))
                {
                    ViewBag.error = "Username exists";
                    return View(account);
                }

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                int id = int.Parse(_context.Accounts.ToList().Last().Id.ToString());
                Account epl = _context.Accounts.FirstOrDefault(s => s.Id == id);
                epl.Salt ??= Guid.NewGuid().ToString();
                epl.Password = epl.Username.ComputeSha256Hash(epl.Salt, epl.Password);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListEmployee));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(account);
        }

        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UpdatePassWord updatepass)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == updatepass.Username);
            if (account == null)
            {
                ViewBag.error = "Account does not exists";
                return View(updatepass);
            }

            var isValid = account.Username.ValidPassword(account.Salt, updatepass.PasswordOld, account.Password);
            if (!isValid)
            {
                ViewBag.error = "Password incorrect";
                return View(updatepass);
            }

            try
            {
                account.Password = account.Username.ComputeSha256Hash(account.Salt, updatepass.PasswordNew);

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                return Redirect(nameof(ListEmployee));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

           

            return View();
        }

        public async Task<IActionResult> DeleteEmployee(long id)
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

            if (emplView.Id == id)
            {
                return Redirect(nameof(ListEmployee));
            }

            if (_context.Employees == null)
            {
                return Problem("Employee is null");
            }

            var empl = await _context.Employees.FindAsync(id);
            if (empl != null)
            {
                _context.Employees.Remove(empl);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListEmployee));
        }

        private string UploadImg(IFormFile file)
        {
            try
            {
                string FileName = file.FileName;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/employee/", uniqueFileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool EmailExists(string email)
        {
            return _context.Employees.Any(e => e.Email == email);
        }

        private bool EmployeeCodeExists(string code)
        {
            return _context.Employees.Any(e => e.EmployeeCode == code);
        }

        private bool AccountExists(long id)
        {
            return _context.Accounts.Any(e => e.EmployeeId == id);
        }

        private bool UsernameExists(string username)
        {
            return _context.Accounts.Any(e => e.Username == username);
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
