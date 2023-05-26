using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;
using StarSecurity.Models;

namespace StarSecurity.Controllers
{
    public class BusinessController : Controller
    {
        private readonly StarSecurityContext _context;

        public BusinessController(StarSecurityContext context)
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

            var res = await _context.Services.ToListAsync();

            return View(res);
        }

        public async Task<IActionResult> Detail(long id)
        {
            if (id == null || _context.Services == null)
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


            var res = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (res == null)
            {
                return NotFound();
            }

            var prd = await _context.Products.ToListAsync();

            var viewData = new ServiceViewModel
            {
                service = res,
                products = prd
            };
            return View(viewData);
        }

        [HttpPost]
        public IActionResult RegisterService(string name, string email, string phone, string address, string description, long serviceId)
        {
            try
            {
                var model = new RegisterService
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Address = address,
                    Description = description,
                    ServiceId = serviceId
                };
                _context.RegisterServices.Add(model);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Success!"
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    code = 400,
                    msg = "Failed!",
                });
            }
        }
    }
}
