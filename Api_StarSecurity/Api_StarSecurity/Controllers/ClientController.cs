using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarSecurity.Common;

namespace Api_StarSecurity.Controllers
{
    public class ClientController : BaseController<ClientController>
    {
        public ClientController(StarSecurityContext context, ILogger<ClientController> logger, IConfiguration config)
            : base(context, logger, config)
        {
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            var res = _context.Clients;

            return Ok(res);
        }

        [HttpGet("Detail")]
        public IActionResult Details([FromQuery] long id)
        {
            var res = _context.Clients.Find(id);

            if (res == null) return NotFound("No data found");

            return Ok(res);
        }


        [HttpPost("Add")]
        public IActionResult Add([FromQuery] ClientModel model)
        {
            var data = new Client
            {
                Name = model.Name,
                Gender = model.Gender,
                Dob = model.Dob,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Description = model.Description,
                Status = model.Status,
            };

            _context.Clients.Add(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }


        [HttpPut("Edit")]
        public IActionResult Edit([FromQuery] ClientModel model)
        {
            var data = _context.Clients.Find(model.Id);
            if (data == null) return NotFound();

            if (data == null) return NotFound();

            data.Name = model.Name;
            data.Gender = model.Gender;
            data.Dob = model.Dob;
            data.Email = model.Email;
            data.Phone = model.Phone;
            data.Address = model.Address;
            data.Description = model.Description;
            data.Status = model.Status;
            data.UpdatedDate = DateTime.Now;

            _context.Clients.Update(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            var data = _context.Clients.Find(id);
            if (data == null) return NotFound("No data found");

            _context.Clients.Remove(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }
    }
}
