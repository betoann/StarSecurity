using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;
using StarSecurity.Models;

namespace StarSecurity.Controllers
{
    public class ProductController : Controller
    {
        private readonly StarSecurityContext _context;

        public ProductController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(long id)
        {
            if (id == null || _context.Recruitments == null)
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


            var res = await _context.Products.FirstOrDefaultAsync(r => r.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            var listprd = await _context.Products.ToListAsync();

            var viewData = new ProductDetailModel
            {
                product = res,
                products = listprd,
            };

            return View(viewData);
        }

        [HttpPost]
        public IActionResult RegisterService(string name, string email, string phone, string address, string description, long prdId)
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
                    ProductId = prdId
                };
                _context.RegisterServices.Add(model);
                _context.SaveChanges();

                return Json(new
                {
                    code = 200,
                    msg = "Success!"
                });
            }
            catch (Exception ex)
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
