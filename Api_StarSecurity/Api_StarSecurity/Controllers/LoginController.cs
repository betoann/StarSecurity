using Api_StarSecurity.Entites;
using Api_StarSecurity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarSecurity.Common;

namespace Api_StarSecurity.Controllers
{
    public class LoginController : BaseController<LoginController>
    {
        public LoginController(StarSecurityContext context, ILogger<LoginController> logger, IConfiguration config)
            : base(context, logger, config)
        {
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromQuery] LoginModel model)
        {
            var data = _context.Accounts.FirstOrDefault(m => m.Username == model.Username);
            if (data == null) return BadRequest("Username/password incorrect");

            var isValid = model.Username.ValidPassword(data.Salt, model.Password, data.Password);
            if (!isValid) return BadRequest("Username/password incorrect");

            var accessToken = GenerateToken(model.Username);
            return Ok(accessToken);
        }


    }
}
