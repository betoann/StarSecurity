using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;
using StaffAssign = StarSecurity.Entites.Task;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly StarSecurityContext _context;

        public TaskController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListTask(int status)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.Tasks.Where(m => status == 0 || m.Status == status).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailTask(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }
            var res = await _context.Tasks
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddTask()
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

            var employee = await _context.Employees.ToListAsync();
            ViewBag.Employee = new SelectList(employee, "Id", "Name");

            var client = await _context.Clients.ToListAsync();
            ViewBag.Client = new SelectList(client, "Id", "Name");

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(StaffAssign task)
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

            var employee = await _context.Employees.ToListAsync();
            ViewBag.Employee = new SelectList(employee, "Id", "Name");

            var client = await _context.Clients.ToListAsync();
            ViewBag.Client = new SelectList(client, "Id", "Name");

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

            try
            {
                if (!EmployeeExists(task.EmployeeId))
                {
                    ViewBag.error = "Employee name does not exists";
                    return View(task);
                }
                if (!ClientExists(task.ClientId))
                {
                    ViewBag.error = "Client name does not exists";
                    return View(task);
                }
                if (!ServiceExists(task.ServiceId))
                {
                    ViewBag.error = "Service name does not exists";
                    return View(task);
                }
                if(task.Description == null)
                {
                    return View(task);
                }

                task.CreateBy = email;
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListTask));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(task);
        }

        public async Task<IActionResult> EditTask(long id)
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

            var employee = await _context.Employees.ToListAsync();
            ViewBag.Employee = new SelectList(employee, "Id", "Name");

            var client = await _context.Clients.ToListAsync();
            ViewBag.Client = new SelectList(client, "Id", "Name");

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(long id, StaffAssign task)
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

            var employee = await _context.Employees.ToListAsync();
            ViewBag.Employee = new SelectList(employee, "Id", "Name");

            var client = await _context.Clients.ToListAsync();
            ViewBag.Client = new SelectList(client, "Id", "Name");

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

            if (id != task.Id)
            {
                return NotFound();
            }
            try
            {
                if (!EmployeeExists(task.EmployeeId))
                {
                    ViewBag.error = "Employee name does not exists";
                    return View(task);
                }
                if (!ClientExists(task.ClientId))
                {
                    ViewBag.error = "Client name does not exists";
                    return View(task);
                }
                if (!ServiceExists(task.ServiceId))
                {
                    ViewBag.error = "Service name does not exists";
                    return View(task);
                }
                if (task.Description == null)
                {
                    return View(task);
                }

                task.UpdateBy = email;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListTask));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return View(task);
        }

        public async Task<IActionResult> DeleteTask(long id)
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

            if (_context.Tasks == null)
            {
                return Problem("Task is null");
            }
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListTask));
        }

        private bool EmployeeExists(long id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        private bool ClientExists(long id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

        private bool ServiceExists(long id)
        {
            return _context.Services.Any(e => e.Id == id);
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
