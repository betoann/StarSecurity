using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Api_StarSecurity.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = Api_StarSecurity.Entites.Task;

namespace Api_StarSecurity.Controllers
{
    public class TaskController : BaseController<TaskController>
    {
        public TaskController(StarSecurityContext context, ILogger<TaskController> logger, IConfiguration config)
            : base(context, logger, config)
        {
        }

        [HttpGet("List")]
        public IActionResult GetList([FromQuery] TaskFilter model)
        {
            var res = _context.Tasks.Where(m => m.Status == model.Status || model.Status == 0);

            return Ok(res);
        }

        [HttpGet("Detail")]
        public IActionResult Details([FromQuery] long id)
        {
            var res = _context.Tasks.Find(id);

            if (res == null) return NotFound("No data found");

            return Ok(res);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromQuery] TaskModel model)
        {
            var dataEmployee = _context.Employees.Find(model.EmployeeId);
            if (dataEmployee == null) return NotFound();

            var dataClient = _context.Clients.Find(model.ClientId);
            if (dataClient == null) return NotFound();

            var assignwork = new Task
            {
                EmployeeId = model.EmployeeId,
                ClientId = model.ClientId,
                ServiceId = model.ServiceId,
                Description = model.Description,
                Status = model.Status
            };

            _context.Tasks.Add(assignwork);
            var rowff = _context.SaveChanges();

            return rowff > 0 ? Ok("Success") : BadRequest("Failed");
        }

        [HttpPut("Edit")]
        public IActionResult Edit([FromQuery] TaskModel model)
        {
            var data = _context.Tasks.Find(model.Id);
            if (data == null) return NotFound();

            var dataEmployee = _context.Employees.Find(model.EmployeeId);
            if (dataEmployee == null) return NotFound();

            var dataClient = _context.Clients.Find(model.ClientId);
            if (dataClient == null) return NotFound();


            data.EmployeeId = model.EmployeeId;
            data.ClientId = model.ClientId;
            data.ServiceId = model.ServiceId;
            data.Description = model.Description;
            data.Status = model.Status;

            _context.Tasks.Update(data);
            var rowff = _context.SaveChanges();

            return rowff > 0 ? Ok("Success") : BadRequest("Failed");
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            var data = _context.Tasks.Find(id);
            if (data == null) return NotFound();

            _context.Tasks.Remove(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }
    }
}
