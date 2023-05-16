using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_StarSecurity.Controllers
{
   public class ServiceController : BaseController<ServiceController>
   {
       public ServiceController(StarSecurityContext context, ILogger<ServiceController> logger, IConfiguration config)
           : base(context, logger, config)
       {
       }

       [HttpGet("List")]
       public IActionResult List()
       {
           var res = _context.Services;

           return Ok(res);
       }

       [HttpGet("Detail")]
       public IActionResult Details([FromQuery] long id)
       {
           var res = _context.Services.Find(id);

           if (res == null) return NotFound("No data found");

           return Ok(res);
       }

        
       [HttpPost("Add")]
       public IActionResult Add([FromQuery] ServiceModel model)
       {
           var data = new Service
           {
               Name = model.Name,
               Image = model.Image,
               Description = model.Description
           };

           _context.Services.Add(data);
           var roweff = _context.SaveChanges();

           return roweff > 0 ? Ok("Success") : BadRequest("Failed");
       }

        
       [HttpPut("Edit")]
       public IActionResult Edit([FromQuery] ServiceModel model)
       {
           var data = _context.Services.Find(model.Id);
           if (data == null) return NotFound();

           data.Name = model.Name;
           data.Image = model.Image;
           data.Description = model.Description;
           data.UpdatedDate = DateTime.Now;

           _context.Services.Update(data);
           var roweff = _context.SaveChanges();

           return roweff > 0 ? Ok("Success") : BadRequest("Failed");
       }

        
       [HttpDelete("Delete")]
       public IActionResult Delete([FromQuery] long id)
       {
           var data = _context.Services.Find(id);
           if (data == null) return NotFound("No data found");

           _context.Services.Remove(data);
           var roweff = _context.SaveChanges();

           return roweff > 0 ? Ok("Success") : BadRequest("Failed");
       }
   }
}
