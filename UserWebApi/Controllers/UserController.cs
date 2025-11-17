using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using UserWebAPI.Dto;
using UserWebAPI.Models;

namespace UserWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(UserDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        //Register an User
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role ?? "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        //Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password");

            // Call AuthenticationWebApi to generate JWT
            var client = _httpClientFactory.CreateClient();
            var jwtRequest = new { Id = user.Id, Email = user.Email, Role = user.Role };

            var response = await client.PostAsJsonAsync("http://authentication-api/api/account/token", jwtRequest);


            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "JWT service unavailable");

            var token = await response.Content.ReadAsStringAsync();
            return Ok(token);
        }


        //Get all users (protected)
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto { Id = u.Id, Name = u.Name, Email = u.Email })
                .ToListAsync();

            return Ok(users);
        }
    }
}
