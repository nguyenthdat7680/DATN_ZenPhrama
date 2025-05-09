using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DA.ZenPharma.Application.Dtos.LoginDto;
using DA.ZenPharma.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ZenPharmaDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ZenPharmaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(b => b.Branch)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.PasswordHash);  

            if (user == null)
            {
                return Unauthorized("Invalid Email or Password");  
            }

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role.RoleName),
                new Claim("IsActive", user.IsActive.ToString()),
                new Claim("userName", user.LastName.ToString())
            };

            if (user.Branch != null)
            {
                claims.Add(new Claim("branchId", user.Branch.Id.ToString()));
                claims.Add(new Claim("branchName", user.Branch.BranchName ?? ""));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenString });
        }

    }
}
