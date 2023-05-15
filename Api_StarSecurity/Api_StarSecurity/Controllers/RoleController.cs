using System.Data;
using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_StarSecurity.Controllers
{
    public class RoleController : BaseController<RoleController>
    {
        public RoleController(StarSecurityContext context, ILogger<RoleController> logger, IConfiguration config)
            : base(context, logger, config)
        {
        }

        [HttpGet("List")]
        public IActionResult GetList()
        {
            var res = _context.Roles;

            return Ok(res);
        }

        [HttpGet("Detail")]
        public IActionResult Details([FromQuery] long id)
        {
            var res = _context.Roles.Find(id);

            if (res == null) return NotFound("No data found");

            return Ok(res);
        }


        [HttpPost("Add")]
        public IActionResult Add([FromQuery] RoleModel model)
        {
            var data = new Role
            {
                Name = model.Name,
            };

            _context.Roles.Add(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }


        [HttpPut("Edit")]
        public IActionResult Edit([FromQuery] RoleModel model)
        {
            var data = _context.Roles.Find(model.Id);
            if (data == null) return NotFound();

            data.Name = model.Name;
            data.UpdatedDate = DateTime.Now;

            _context.Roles.Update(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }


        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            var data = _context.Roles.Find(id);
            if (data == null) return NotFound("No data found");

            _context.Roles.Remove(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }
    }
}
