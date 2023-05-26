using System.Threading.Tasks;
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

        public async Task<IActionResult> ListRecruitment()
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

            var recruit = await _context.Recruitments.ToListAsync();

            return View(recruit);
        }

        public async Task<IActionResult> DetailRecruitment(long id)
        {
            var email = HttpContext.Request.Cookies["email"];
            var emplView = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            ViewBag.EmployeeEmail = email;
            ViewBag.EmployeeAvatar = emplView.Avatar;
            ViewBag.EmployeeName = emplView.Name;
            ViewBag.EmployeeId = emplView.Id;

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

            var service = await _context.Services.ToListAsync();
            ViewBag.Service = new SelectList(service, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRecruitment(Recruitment recruitment)
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

            try
            {
                if (recruitment.Description == null || recruitment.Request == null || recruitment.Benefit == null)
                {
                    return View(recruitment);
                }

                recruitment.CreateBy = email;
                _context.Recruitments.Add(recruitment);
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

            if (id != recruitment.Id)
            {
                return NotFound();
            }

            try
            {
                if (recruitment.Description == null || recruitment.Request == null || recruitment.Benefit == null)
                {
                    return View(recruitment);
                }

                recruitment.UpdateBy = email;
                recruitment.UpdatedDate = DateTime.Now;
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


        //Candidate

        public async Task<IActionResult> ListCandidate(int status, long serviceId)
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

            var candidate = await _context.Candidates.Where(m => (status == 0 || m.Status == status)).ToListAsync();

            return View(candidate);
        }

        public async Task<IActionResult> DetailCandidate(long id)
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

            if (id == null || _context.Candidates == null)
            {
                return NotFound();
            }
            var res = await _context.Candidates
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> ReadedCandidate(long id)
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

            try
            {
                var cdd = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == id);
                cdd.Status = 1;
                _context.Candidates.Update(cdd);
                await _context.SaveChangesAsync();


                await _context.SaveChangesAsync();
                return Redirect(nameof(ListCandidate));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Redirect(nameof(ListCandidate));
        }

        public async Task<IActionResult> DeleteCandidate(long id)
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

            if (_context.Recruitments == null)
            {
                return Problem("Candidates is null");
            }
            var cdd = await _context.Candidates.FindAsync(id);
            if (cdd != null)
            {
                _context.Candidates.Remove(cdd);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListCandidate));
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
