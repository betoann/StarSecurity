using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Controllers
{
    public class OurDepartmentController : Controller
    {
        private readonly StarSecurityContext _context;

        public OurDepartmentController(StarSecurityContext context)
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

            var res = await _context.Departments.Where(s => s.Status == 1).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailDepartment(long id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var service = await _context.Services.Where(s => s.Status == 1).ToListAsync();
            Dictionary<long, string> sev = new Dictionary<long, string>();
            foreach (var item in service)
            {
                sev.Add(item.Id, item.Name);
            }

            ViewBag.CategoryService = sev;

            var res = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }
    }
}
