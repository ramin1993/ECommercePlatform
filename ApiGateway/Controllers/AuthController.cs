using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] RequestLogin requestLogin)
        {
            if (requestLogin.Username == "user" && requestLogin.Password == "pass")
            {
                var token = GenerateJwtToken(requestLogin.Username, "User");
                return Ok(new { Token = token });
            }
            else if (requestLogin.Username == "admin" && requestLogin.Password == "admin")
            {
                var token = GenerateJwtToken(requestLogin.Username, "Admin");
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
        private string GenerateJwtToken(string username,string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // "ApiGateway"
                audience: _configuration["Jwt:Audience"], // "OrderAndProductServices"
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
    public class RequestLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
