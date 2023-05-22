using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Controllers
{
    public class OurbusinessController : Controller
    {
        private readonly StarSecurityContext _context;

        public OurbusinessController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var res = await _context.Services.Where(s => s.Status == 1).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailBusiness(long id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var res = await _context.Services
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }
    }
}
