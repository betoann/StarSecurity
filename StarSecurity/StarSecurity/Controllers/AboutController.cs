using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Controllers
{
    public class AboutController : Controller
    {
        private readonly StarSecurityContext _context;

        public AboutController(StarSecurityContext context)
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
    }
}
