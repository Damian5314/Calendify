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
        private readonly DatabaseContext _context;

        public LoginController(ILoginService loginService, DatabaseContext context)
        {
            _loginService = loginService;
            _context = context;
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
            var status = _loginService.CheckPassword(loginRequest.Email, loginRequest.Password);

            if (status == LoginStatus.Success)
            {
                var user = _context.User.FirstOrDefault(u => u.Email == loginRequest.Email);
                if (user != null)
                {
                    // Set all required session values
                    HttpContext.Session.SetString("Role", "User");
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("LastName", user.LastName);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetInt32("UserId", user.UserId); // Store UserId for other queries

                    return Ok(new { Role = "User", FirstName = user.FirstName });
                }
            }

            return Unauthorized("Invalid email or password.");
        }

        // Get User Info
        [HttpGet("user-info")]
        public IActionResult GetUserInfo()
        {
            var firstName = HttpContext.Session.GetString("FirstName");
            var role = HttpContext.Session.GetString("Role");
            var lastName = HttpContext.Session.GetString("LastName");
            var email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(role))
            {
                return Unauthorized("Session has expired or user is not logged in.");
            }

            return Ok(new { FirstName = firstName, Role = role, LastName = lastName, Email = email });
        }

        // Update Email
        [HttpPut("update-email")]
        public IActionResult UpdateEmail([FromBody] UpdateEmailRequest request)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var user = _context.User.Find(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Email = request.Email;
            _context.SaveChanges();

            HttpContext.Session.SetString("Email", request.Email); // Update session
            return Ok("Email updated successfully.");
        }

        // Update Password
        [HttpPut("update-password")]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var user = _context.User.Find(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Password != request.CurrentPassword) // Adjust for hashing logic if applicable
            {
                return BadRequest("Current password is incorrect.");
            }

            user.Password = request.NewPassword; // Hash the password before saving if needed
            _context.SaveChanges();

            return Ok("Password updated successfully.");
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

    // Request Models for Updates
    public class UpdateEmailRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class UpdatePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}