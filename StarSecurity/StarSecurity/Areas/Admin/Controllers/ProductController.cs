using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly StarSecurityContext _context;

        public ProductController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListProduct()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var res = await _context.Products.ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailProduct(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var res = await _context.Products
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddProduct()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            var partner = await _context.Partners.ToListAsync();
            ViewBag.Partner = new SelectList(partner, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product Product, IFormFile fileImage)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            var partner = await _context.Partners.ToListAsync();
            ViewBag.Partner = new SelectList(partner, "Id", "Name");

            try
            {
                Product.CreateBy = email;
                _context.Products.Add(Product);
                await _context.SaveChangesAsync();

                if(fileImage != null)
                {
                    int id = int.Parse(_context.Products.ToList().Last().Id.ToString());
                    Product prd = _context.Products.FirstOrDefault(s => s.Id == id);
                    prd.Image = UploadImg(fileImage);
                    await _context.SaveChangesAsync();
                }

                return Redirect(nameof(ListProduct));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Product);
        }

        public async Task<IActionResult> EditProduct(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            var partner = await _context.Partners.ToListAsync();
            ViewBag.Partner = new SelectList(partner, "Id", "Name");

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(long id, Product Product, IFormFile fileImage)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            var partner = await _context.Partners.ToListAsync();
            ViewBag.Partner = new SelectList(partner, "Id", "Name");

            if (id != Product.Id)
            {
                return NotFound();
            }

            try
            {
                if (fileImage != null)
                {
                    Product.Image = UploadImg(fileImage);
                }

                Product.UpdateBy = email;
                Product.UpdatedDate = DateTime.Now;
                _context.Products.Update(Product);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListProduct));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(Product);
        }

        public async Task<IActionResult> DeleteProduct(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            if (!CheckRoleAdmin(email))
            {
                return RedirectToRoute("PageError");
            }

            if (_context.Products == null)
            {
                return Problem("Product is null");
            }
            var Product = await _context.Products.FindAsync(id);
            if (Product != null)
            {
                _context.Products.Remove(Product);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListProduct));
        }

        private string UploadImg(IFormFile file)
        {
            try
            {
                string FileName = file.FileName;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/product/", uniqueFileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckRoleAdmin(string email)
        {
            var empl = _context.Employees.SingleOrDefault(e => e.Email == email);
            if (empl.RoleCode == "RoleAdmin")
            {
                return true;
            }
            return false;
        }
    }
}
