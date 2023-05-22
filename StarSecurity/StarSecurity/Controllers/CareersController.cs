using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Controllers
{
    public class CareersController : Controller
    {
        private readonly StarSecurityContext _context;

        public CareersController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var res = await _context.Recruitments.Where(r => r.Status == 1).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> Detail(long id)
        {
            if (id == null || _context.Recruitments == null)
            {
                return NotFound();
            }
            var res = await _context.Recruitments.FirstOrDefaultAsync(r => r.Id == id);
            if(res == null)
            {
                return NotFound();
            }
            return View(res);
        }
    }
}
