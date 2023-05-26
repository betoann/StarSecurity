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
            var service = await _context.Services.Where(s => s.Status == 1).ToListAsync();
            Dictionary<long, string> sev = new Dictionary<long, string>();
            foreach (var item in service)
            {
                sev.Add(item.Id, item.Name);
            }

            ViewBag.CategoryService = sev;

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
