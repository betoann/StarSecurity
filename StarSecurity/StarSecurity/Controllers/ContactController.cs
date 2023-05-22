using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Controllers
{
    public class ContactController : Controller
    {
        private readonly StarSecurityContext _context;

        public ContactController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Contact contact)
        {
            try
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Contact");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(contact);
        }

    }
}
