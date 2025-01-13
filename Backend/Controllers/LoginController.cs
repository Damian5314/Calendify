using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Voor toegang tot Entity Framework functies
using StarterKit.Models;
using StarterKit.Services;
using System.Text.RegularExpressions; // Voor Regex validatie
using BCrypt.Net; // Voor wachtwoord hashing

namespace StarterKit.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _context; // Injected DatabaseContext
        private readonly ILoginService _loginService; // Injected LoginService

        public LoginController(DatabaseContext context, ILoginService loginService)
        {
            _context = context; // Initialiseer DatabaseContext
            _loginService = loginService; // Initialiseer LoginService
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

        // Reset Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and Password are required.");

            // Validate the password against the rules (e.g., at least 8 chars, 1 uppercase, 1 number)
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");
            if (!passwordRegex.IsMatch(request.Password))
                return BadRequest("Password must contain at least 8 characters, one uppercase letter, and one number.");

            // Fetch user by email
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return NotFound("User not found.");

            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Save changes to the database
            _context.User.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Password successfully changed.");
        }
    }
}
