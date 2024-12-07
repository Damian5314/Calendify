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

        /// <summary>
        /// Logs in a user with email and password.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return BadRequest("Email and password are required.");

            var loginStatus = _loginService.CheckPassword(loginRequest.Email, loginRequest.Password);

            if (loginStatus == LoginStatus.IncorrectEmail)
                return Unauthorized("Invalid email.");

            if (loginStatus == LoginStatus.IncorrectPassword)
                return Unauthorized("Invalid password.");

            var userId = _loginService.GetUserIdByEmail(loginRequest.Email);
            var role = _loginService.GetUserRoleByEmail(loginRequest.Email);

            // Set session values
            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetString("Role", role); // "Admin" or "User"

            return Ok(new { message = $"Logged in as {role}.", userId });
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logged out successfully.");
        }

        /// <summary>
        /// Check if a user is logged in and their role.
        /// </summary>
        [HttpGet("is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            if (!CheckSessionLoggedIn())
                return Unauthorized("You are not logged in.");

            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");

            return Ok(new { loggedIn = true, role, userId });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _loginService.RegisterUser(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password,
                user.RecuringDays,
                user.Role // "Admin" or "User"
            );

            if (!success)
                return Conflict("Email is already in use.");

            return Ok("User registered successfully.");
        }

        [HttpGet("debug-session")]
        public IActionResult DebugSessionCookies()
        {
            var sessionId = HttpContext.Request.Cookies[".AspNetCore.Session"];
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            return Ok(new
            {
                SessionId = sessionId ?? "Session cookie not found",
                UserId = userId?.ToString() ?? "UserId not set",
                Role = role ?? "Role not set"
            });
        }


        // Helper method to check session login
        private bool CheckSessionLoggedIn() => HttpContext.Session.GetInt32("UserId") != null;
    }
}
