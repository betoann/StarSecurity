using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_StarSecurity.Controllers
{
    public class RecruitmentController : BaseController<RecruitmentController>
    {
        public RecruitmentController(StarSecurityContext context, ILogger<RecruitmentController> logger, IConfiguration config)
            : base(context, logger, config)
        {
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            var res = _context.Recruitments;

            return Ok(res);
        }

        [HttpGet("Detail")]
        public IActionResult Details([FromQuery] long id)
        {
            var res = _context.Recruitments.Find(id);

            if (res == null) return NotFound("No data found");

            return Ok(res);
        }


        [HttpPost("Add")]
        public IActionResult Add([FromQuery] RecruitmentModel model)
        {
            var dataDepartment = _context.Clients.Find(model.ServiceId);
            if (dataDepartment == null) return NotFound();

            var data = new Recruitment
            {
                Vacancies = model.Vacancies,
                Count = model.Count,
                Description = model.Description,
                ServiceId = model.ServiceId,
                Status = model.Status,
            };

            _context.Recruitments.Add(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }


        [HttpPut("Edit")]
        public IActionResult Edit([FromQuery] RecruitmentModel model)
        {
            var data = _context.Recruitments.Find(model.Id);
            if (data == null) return NotFound();

            var dataDepartment = _context.Clients.Find(model.ServiceId);
            if (dataDepartment == null) return NotFound();

            data.Vacancies = model.Vacancies;
            data.Count = model.Count;
            data.Description = model.Description;
            data.ServiceId = model.ServiceId;
            data.Status = model.Status;
            data.UpdatedDate = DateTime.Now;

            _context.Recruitments.Update(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }


        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            var data = _context.Recruitments.Find(id);
            if (data == null) return NotFound("No data found");

            _context.Recruitments.Remove(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }
    }
}
