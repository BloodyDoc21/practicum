using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CleanLife.Web.Models;
using CleanLife.Web.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanLife.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager
                .CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                message = "Пользователь зарегистрирован"
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            var user =
                await _userManager
                .FindByNameAsync(model.Username);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Неверный логин или пароль"
                });
            }

            var validPassword =
                await _userManager
                .CheckPasswordAsync(
                    user,
                    model.Password);

            if (!validPassword)
            {
                return Unauthorized(new
                {
                    message = "Неверный логин или пароль"
                });
            }

            var token = GenerateJwtToken(user);
            var refreshToken = Guid.NewGuid().ToString();

            Response.Cookies.Append(
                "refreshToken",
                refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            return Ok(new
            {
                accessToken = token
            });
        }

        private string GenerateJwtToken(User user)
        {
            Console.WriteLine(_configuration["Jwt:Key"]);
            Console.WriteLine(_configuration["Jwt:Issuer"]);
            Console.WriteLine(_configuration["Jwt:Audience"]);
            Console.WriteLine(_configuration["Jwt:Key"]);
            var jwtKey =
                _configuration["Jwt:Key"];

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName)


            };

            var token =
                new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}