using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;
using StarSecurity.Models.ViewModel;

namespace StarSecurity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StarSecurityContext _context; 

        public HomeController(ILogger<HomeController> logger, StarSecurityContext context)
        {
            _logger = logger;
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

            var department = await _context.Departments.Where(d => d.Status == 1).ToListAsync();
            var partner = await _context.Partners.Where(p => p.Status == 1).ToListAsync();
            var product = await _context.Products.ToListAsync();

            var viewData = new HomeViewModel
            {
                departments = department,
                partners = partner,
                products = product,
            };

            return View(viewData);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}