using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        // User Registration
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Email and Password are required.");

            var isRegistered = _loginService.RegisterUser(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password,
                user.RecuringDays
            );

            if (!isRegistered)
                return Conflict("User with the given email already exists.");

            return Ok("User registered successfully.");
        }

        // User & Admin Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var (isAdmin, role) = _loginService.AdminLogin(loginRequest.Email, loginRequest.Password);

            if (isAdmin)
            {
                HttpContext.Session.SetString("Role", role);
                return Ok(new { Role = role });
            }

            var status = _loginService.CheckPassword(loginRequest.Email, loginRequest.Password);
            if (status == LoginStatus.Success)
            {
                HttpContext.Session.SetString("Role", "User");
                return Ok(new { Role = "User" });
            }

            return Unauthorized("Invalid email or password.");
        }

        // Admin Registration
        [HttpPost("register-admin")]
        public IActionResult RegisterAdmin([FromBody] Admin admin)
        {
            try
            {
                _loginService.RegisterAdmin(admin.UserName, admin.Email, admin.Password);
                return Ok("Admin registered successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // Check if Logged In
        [HttpGet("is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
                return Unauthorized("You are not logged in.");

            return Ok($"Logged in as {role}");
        }

        // Logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logged out successfully.");
        }
    }
}
