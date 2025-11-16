using Microsoft.AspNetCore.Mvc;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;

namespace AuthenticationWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        public AccountController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        //Generates token from email + role
        [HttpPost("token")]
        public ActionResult<AuthenticationResponse?> GenerateToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Role))
                return BadRequest("Invalid request");

            var response = _jwtTokenHandler.GenerateJwtToken(request.Id, request.Email, request.Role);
            return Ok(response);
        }
    }

    public class TokenRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
