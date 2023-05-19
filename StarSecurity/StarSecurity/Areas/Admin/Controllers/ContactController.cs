using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarSecurity.Entites;
using StarSecurity.Models;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactController : Controller
    {
        private readonly StarSecurityContext _context;

        public ContactController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListContact()
        {
            var res = await _context.Contacts.ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailContact(long id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }
            var res = await _context.Contacts
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> DeleteContact()
        {
            return View();
        }
    }
}
