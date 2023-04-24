using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly CoolmateContext _context;
        public AuthController(IConfiguration configuration, CoolmateContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            if (model != null && model.Email != null && model.Password != null)
            {
                var user = await GetUser(model.Email, model.Password);
                if (user != null)
                {
                    var claims = new[]
                    {
                         new Claim("id", user.Id.ToString()),
                         new Claim("userName", user.UserName),
                         new Claim("role", user.Role),
                         new Claim("email", user.Email),
                         new Claim("phone", user.Phone),
                         new Claim("address", user.Address),
                         new Claim("dateOfBirth", user.DateOfBirth.ToString()),
                         new Claim("sex", user.Sex.ToString()),
                         new Claim("height", user.Height.ToString()),
                         new Claim("weight", user.Weight.ToString())
                     };
                    var identity = new ClaimsIdentity(claims, "AuthenticationType");
                    identity.AddClaims(claims); var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds); return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
                return BadRequest();
        }

        private async Task<User> GetUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            // Xoá thông tin đăng nhập của người dùng (token, session, ...)
            HttpContext.Response.Cookies.Delete("access_token"); // xoá cookie access_token

            return Ok();
        }
    }
}
