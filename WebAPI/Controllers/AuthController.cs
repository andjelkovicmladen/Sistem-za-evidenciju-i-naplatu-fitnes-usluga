using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Prijava administratora
        /// </summary>
        /// <param name="loginRequest">Email i lozinka</param>
        /// <returns>JWT token ako je prijava uspešna</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return BadRequest("Email i lozinka su obavezni.");

            var (success, admin, token, error) = _authService.Login(loginRequest.Email, loginRequest.Password);

            if (!success)
                return Unauthorized(new { message = error });

            return Ok(new { token, admin = new { admin.IdAdministrator, admin.ImePrezime, admin.Email } });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
