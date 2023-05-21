﻿using System.Security.Principal;
using System.Web.Helpers;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarSecurity.Common;
using StarSecurity.Entites;

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

        public async Task<IActionResult> ListEmployee(int status, long serviceId, string name)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var employee = await _context.Employees.Where(m => (status == 0 || m.Status == status)
                                                    && (serviceId == 0 || m.ServiceId == serviceId)
                                                    && (string.IsNullOrEmpty(name) || m.Name.ToLower().Contains(name.ToLower()))).ToListAsync();

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");
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

            var tasks = await _context.Tasks.Where(m => m.EmployeeId == id).ToListAsync();

            var viewData = new EmployeeDetail
            {
                Employee = res,
                Tasks = tasks
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

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

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

                employee.CreateBy = email;
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                int id = int.Parse(_context.Employees.ToList().Last().Id.ToString());
                Employee epl = _context.Employees.FirstOrDefault(s => s.Id == id);
                epl.Avatar = UploadImg(fileAvatar);
                await _context.SaveChangesAsync();

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
            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

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

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

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
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/employee/", FileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return FileName;
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
