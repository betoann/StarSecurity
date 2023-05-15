using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api_StarSecurity.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api_StarSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        protected readonly IConfiguration _config;
        protected readonly StarSecurityContext _context;
        protected readonly ILogger<T> _logger;

        public BaseController(StarSecurityContext context, ILogger<T> logger,
            IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }

        protected string GenerateToken(string username)
        {
            var secretKey = _config.GetValue<string>("Authen:Secret");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
