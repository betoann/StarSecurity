using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RecruitmentController : Controller
    {
        private readonly StarSecurityContext _context;

        public RecruitmentController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListRecruitment(int status, long serviceId)
        {
            var recruit = await _context.Recruitments.ToListAsync();

            recruit = await _context.Recruitments.Where(m => (status == 0 || m.Status == status)
                                                                && (serviceId == 0 || m.ServiceId == serviceId)).ToListAsync();

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

            return View(recruit);
        }

        public async Task<IActionResult> DetailRecruitment(long id)
        {
            if (id == null || _context.Recruitments == null)
            {
                return NotFound();
            }
            var res = await _context.Recruitments
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddRecruitment()
        {
            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRecruitment(Recruitment recruitment, IFormFile fileImage)
        {
            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

            try
            {
                _context.Recruitments.Add(recruitment);
                await _context.SaveChangesAsync();

                int id = int.Parse(_context.Recruitments.ToList().Last().Id.ToString());
                Recruitment rcm = _context.Recruitments.FirstOrDefault(s => s.Id == id);
                rcm.Image = UploadImg(fileImage);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListRecruitment));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(recruitment);
        }

        public async Task<IActionResult> EditRecruitment(long id)
        {
            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");

            if (id == null || _context.Recruitments == null)
            {
                return NotFound();
            }

            var rcm = await _context.Recruitments.FindAsync(id);
            if (rcm == null)
            {
                return NotFound();
            }
            return View(rcm);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecruitment(long id, Recruitment recruitment)
        {
            if (id != recruitment.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Recruitments.Update(recruitment);
                await _context.SaveChangesAsync();

                return Redirect(nameof(ListRecruitment));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(recruitment);
        }

        public async Task<IActionResult> DeleteRecruitment(long id)
        {
            if (_context.Recruitments == null)
            {
                return Problem("Recruitment is null");
            }
            var rcm = await _context.Recruitments.FindAsync(id);
            if (rcm != null)
            {
                _context.Recruitments.Remove(rcm);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListRecruitment));
        }

        private string UploadImg(IFormFile file)
        {
            try
            {
                string FileName = file.FileName;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/recruitment/", FileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return FileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
