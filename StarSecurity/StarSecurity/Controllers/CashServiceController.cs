using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;
using StarSecurity.Models;
using StarSecurity.Services;

namespace StarSecurity.Controllers
{
    public class CashServiceController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly StarSecurityContext _context;

        public CashServiceController(IVnPayService vnPayService, StarSecurityContext context)
        {
            _vnPayService = vnPayService;
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
        public IActionResult RegisterService(string name, string email, string phone, string address, string description)
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

        public async Task<IActionResult> Transfer()
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

        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if(response.VnPayResponseCode == "00")
            {
                if (EmailExists(response.Email))
                {
                    var client = await _context.CashServices.FirstOrDefaultAsync(c => c.Email == response.Email);
                    var amountCurrent = client.Amount;
                    client.Amount = amountCurrent + response.Amount;
                    await _context.SaveChangesAsync();

                    return Redirect(nameof(Success));
                }
                else
                {
                    var client = new CashService
                    {
                        Name = response.Name,
                        Email = response.Email,
                        Phone = response.Phone,
                        Address = response.Address,
                        Amount = response.Amount,
                        PaymentMethod = response.PaymentMethod,
                        Description = response.OrderDescription,
                        OrderId = response.OrderId,
                        PaymentId = response.PaymentId,
                        TransactionId = response.TransactionId
                    };
                    _context.CashServices.Add(client);
                     await _context.SaveChangesAsync();

                    return Redirect(nameof(Success));
                }
            }

            return Redirect(nameof(Failed));
        }

        public async Task<IActionResult> Success()
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

        public async Task<IActionResult> Failed()
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

        private bool EmailExists(string email)
        {
            return _context.CashServices.Any(e => e.Email == email);
        }
    }
}
