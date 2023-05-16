using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Api_StarSecurity.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_StarSecurity.Controllers
{
    public class ContactController : BaseController<ContactController>
    {
        public ContactController(StarSecurityContext context, ILogger<ContactController> logger, IConfiguration config)
            : base(context, logger, config)
        {
        }

        [HttpGet("List")]
        public IActionResult GetList([FromQuery] ContactFilter model)
        {
            var res = _context.Contacts.Where(m => m.Status == model.Status || model.Status == 0);

            return Ok(res);
        }

        [HttpGet("Detail")]
        public IActionResult Details([FromQuery] long id)
        {
            var res = _context.Contacts.Find(id);

            if (res == null) return NotFound("No data found");

            return Ok(res);
        }


        [HttpPost("Add")]
        public IActionResult Add([FromQuery] ContactModel model)
        {
            var data = new Contact
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Message = model.Message,
                Status = model.Status,
            };

            _context.Contacts.Add(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }


        [HttpPut("Edit")]
        public IActionResult Edit([FromQuery] ContactModel model)
        {
            var data = _context.Contacts.Find(model.Id);
            if (data == null) return NotFound();

            if (data == null) return NotFound();

            data.Name = model.Name;
            data.Email = model.Email;
            data.Phone = model.Phone;
            data.Address = model.Address;
            data.Message = model.Message;
            data.Status = model.Status;

            _context.Contacts.Update(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            var data = _context.Contacts.Find(id);
            if (data == null) return NotFound("No data found");

            _context.Contacts.Remove(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }
    }
}
