using System.Net;
using System.Web.Helpers;
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

        public async Task<IActionResult> Index(string s)
        {
            var service = await _context.Services.Where(s => s.Status == 1).ToListAsync();
            Dictionary<long, string> sev = new Dictionary<long, string>();
            foreach (var item in service)
            {
                sev.Add(item.Id, item.Name);
            }
            ViewBag.CategoryService = sev;

            var res = await _context.Recruitments.Where(r => (r.Status == 1)
                                                            && (r.TimeEnd > DateTime.Now)).ToListAsync();
            if (s != null)
            {
                res = await _context.Recruitments.Where(r => (r.Status == 1)
                                                                && (r.TimeEnd > DateTime.Now)
                                                                && (r.Title.ToLower().Contains(s.ToLower()))).ToListAsync();
            }
            return View(res);
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

            var res = await _context.Recruitments.FirstOrDefaultAsync(r => r.Id == id);
            if(res == null || res.TimeEnd < DateTime.Now)
            {
                return NotFound();
            }
            return View(res);
        }

        [HttpPost]
        public IActionResult SubmitCV(string name, string email, long rcmId, IFormFile fileCV, string message)
        {
            try
            {
                var model = new Candidate
                {
                    Name = name,
                    Email = email,
                    RecruitmentId = rcmId,
                    FileCv = Uploadfile(fileCV),
                    Message = message
                };
                _context.Candidates.Add(model);
                _context.SaveChanges();

                return Redirect(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string Uploadfile(IFormFile file)
        {
            try
            {
                string FileName = file.FileName;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/cv/", uniqueFileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
