using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly StarSecurityContext _context;

        public RoleController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListRole()
        {
            var res = await _context.Roles.ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (RoleExists(role.Name))
                    {
                        ViewBag.error = "Role name exists";
                        return View(role);
                    }
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return Redirect(nameof(ListRole));
            }
            return View(role);
        }

        public async Task<IActionResult> EditRole(long id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(long id, Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (RoleExists(role.Name))
                    {
                        ViewBag.error = "Role name exists";
                        return View(role);
                    }
                    _context.Roles.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return Redirect(nameof(ListRole));
            }
            return View(role);
        }

        public async Task<IActionResult> DeleteRole(long id)
        {
            if (_context.Roles == null)
            {
                return Problem("Role is null");
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListRole));
        }

        private bool RoleExists(string name)
        {
            return _context.Roles.Any(e => e.Name == name);
        }
    }
}
