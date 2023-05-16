using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Security.Principal;
using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Api_StarSecurity.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarSecurity.Common;
using Account = Api_StarSecurity.Entites.Account;

namespace Api_StarSecurity.Controllers
{
    public class EmployeeController : BaseController<EmployeeController>
    {
        public EmployeeController(StarSecurityContext context, ILogger<EmployeeController> logger, IConfiguration config)
            : base(context, logger, config)
        {
        }

        [HttpGet("List")]
        public IActionResult GetList([FromQuery] EmployeeFilter model)
        {
            var res = _context.Employees.Where(m => (m.Status == model.Status || model.Status == 1)
                                                &&(m.Name.ToLower().Contains(model.Name.ToLower()) || model.Name == ""));

            return Ok(res);
        }

        [HttpGet("Detail")]
        public IActionResult Details([FromQuery] long id)
        {
            var res = _context.Employees.Find(id);

            if (res == null) return NotFound("No data found");

            return Ok(res);
        }


        [HttpPost("Add")]
        public IActionResult Add([FromQuery] EmployeeModel model)
        {
            var dataDepartment = _context.Services.Find(model.ServiceId);
            if (dataDepartment == null) return NotFound();

            var dataRole = _context.Roles.Find(model.RoleId);
            if (dataRole == null) return NotFound();

            var data = new Employee
            {
                Name = model.Name,
                Avatar = model.Avatar,
                Gender = model.Gender,
                Dob = model.Dob,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                EduQualifi = model.EduQualifi,
                EmployeeCode = model.EmployeeCode,
                ServiceId = model.ServiceId,
                RoleId = model.RoleId,
                Grade = model.Grade,
                Achievements = model.Achievements,
                Status = model.Status,
            };

            _context.Employees.Add(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }


        [HttpPut("Edit")]
        public IActionResult Edit([FromQuery] EmployeeModel model)
        {
            var data = _context.Employees.Find(model.Id);
            if (data == null) return NotFound();

            var dataDepartment = _context.Services.Find(model.ServiceId);
            if (dataDepartment == null) return NotFound();

            var dataRole = _context.Roles.Find(model.RoleId);
            if (dataRole == null) return NotFound();

            data.Name = model.Name;
            data.Avatar = model.Avatar;
            data.Gender = model.Gender;
            data.Dob = model.Dob;
            data.Email = model.Email;
            data.Phone = model.Phone;
            data.Address = model.Address;
            data.EduQualifi = model.EduQualifi;
            data.EmployeeCode = model.EmployeeCode;
            data.ServiceId = model.ServiceId;
            data.RoleId = model.RoleId;
            data.Grade = model.Grade;
            data.Achievements = model.Achievements;
            data.Status = model.Status;
            data.UpdatedDate = DateTime.Now;

            _context.Employees.Update(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }

        [HttpPost("RegisterAccount")]
        public IActionResult RegisterAccount([FromQuery] AccountModel model)
        {
            var data = _context.Employees.Find(model.EmployeeId);
            if (data == null) return NotFound("No data found");

            var dataAcc = new Account
            {
                Username = model.Username,
                Password = model.Password,
                EmployeeId = (long)model.EmployeeId,
            };

            dataAcc.Salt ??= Guid.NewGuid().ToString();

            var passHash = dataAcc.Username.ComputeSha256Hash(dataAcc.Salt, model.Password);

            dataAcc.Password = passHash;

            _context.Accounts.Add(dataAcc);
            var roweff = _context.SaveChanges();
            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] long id)
        {
            var data = _context.Employees.Find(id);
            if (data == null) return NotFound("No data found");

            _context.Employees.Remove(data);
            var roweff = _context.SaveChanges();

            return roweff > 0 ? Ok("Success") : BadRequest("Failed");
        }
    }
}
