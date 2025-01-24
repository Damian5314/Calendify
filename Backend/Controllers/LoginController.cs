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
            Console.WriteLine("[RegisterUser] Registration request received.");

            // Validate input
            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                Console.WriteLine("[RegisterUser] Validation failed: Missing required fields.");
                return BadRequest(new { Message = "All fields (First Name, Last Name, Email, Password) are required." });
            }

            // Attempt registration
            var isRegistered = _loginService.RegisterUser(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password,
                user.RecuringDays
            );

            if (!isRegistered)
            {
                Console.WriteLine("[RegisterUser] Registration failed: Email already exists.");
                return Conflict(new { Message = "A user with the given email already exists." });
            }

            Console.WriteLine("[RegisterUser] Registration successful.");
            return Ok(new { Message = "User registered successfully." });
        }

        // User & Admin Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { Message = "Email and Password are required." });
            }

            // Check for Admin login
            var (isAdmin, role) = _loginService.AdminLogin(loginRequest.Email, loginRequest.Password);
            if (isAdmin)
            {
                var userId = _loginService.GetUserIdByEmail(loginRequest.Email);
                var firstName = _loginService.GetFirstNameByEmail(loginRequest.Email);
                HttpContext.Session.SetString("Role", role);

                return Ok(new { Message = "Login successful.", Role = role, UserId = userId, FirstName = firstName });
            }

            // Check for User login
            var status = _loginService.CheckPassword(loginRequest.Email, loginRequest.Password);
            if (status == LoginStatus.Success)
            {
                var userId = _loginService.GetUserIdByEmail(loginRequest.Email);
                var firstName = _loginService.GetFirstNameByEmail(loginRequest.Email);
                HttpContext.Session.SetString("Role", "User");

                return Ok(new { Message = "Login successful.", Role = "User", UserId = userId, FirstName = firstName });
            }

            return Unauthorized(new { Message = "Invalid email or password." });
        }


        // Admin Registration
        [HttpPost("register-admin")]
        public IActionResult RegisterAdmin([FromBody] Admin admin)
        {
            Console.WriteLine("[RegisterAdmin] Admin registration request received.");

            // Validate input
            if (string.IsNullOrWhiteSpace(admin.UserName) || string.IsNullOrWhiteSpace(admin.Email) ||
                string.IsNullOrWhiteSpace(admin.Password))
            {
                Console.WriteLine("[RegisterAdmin] Validation failed: Missing required fields.");
                return BadRequest(new { Message = "All fields (Username, Email, Password) are required." });
            }

            try
            {
                _loginService.RegisterAdmin(admin.UserName, admin.Email, admin.Password);
                Console.WriteLine($"[RegisterAdmin] Admin '{admin.Email}' registered successfully.");
                return Ok(new { Message = "Admin registered successfully." });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[RegisterAdmin] Registration failed: {ex.Message}");
                return Conflict(new { Message = ex.Message });
            }
        }

        // Check if Logged In
        [HttpGet("is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            Console.WriteLine("[IsLoggedIn] Checking if user is logged in.");
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                Console.WriteLine("[IsLoggedIn] User is not logged in.");
                return Unauthorized(new { Message = "You are not logged in." });
            }

            Console.WriteLine($"[IsLoggedIn] User logged in as: {role}");
            return Ok(new { Message = $"Logged in as {role}", Role = role });
        }

        // Logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Console.WriteLine("[Logout] Logging out user.");
            HttpContext.Session.Clear();
            return Ok(new { Message = "Logged out successfully." });
        }

        // Forgot Password: Generate Reset Token
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            Console.WriteLine($"[ForgotPassword] Password reset request for email: {email}");

            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("[ForgotPassword] Validation failed: Missing email.");
                return BadRequest(new { Message = "Email is required." });
            }

            var result = _loginService.GeneratePasswordResetToken(email);
            if (!result)
            {
                Console.WriteLine("[ForgotPassword] Email not found in the system.");
                return NotFound(new { Message = "Email not found in the system." });
            }

            Console.WriteLine("[ForgotPassword] Password reset link sent.");
            return Ok(new { Message = "A password reset link has been sent to your email." });
        }
        [HttpGet("user/profile")]
        public IActionResult GetUserProfile()
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");
            var firstName = HttpContext.Session.GetString("FirstName");

            if (string.IsNullOrEmpty(role) || userId == null)
            {
                return Unauthorized(new { Message = "You must be logged in to access this resource." });
            }

            return Ok(new
            {
                Role = role,
                UserId = userId,
                FirstName = firstName
            });
        }
        // Reset Password
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            Console.WriteLine($"[ResetPassword] Reset password request for token: {request.Token}");

            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                Console.WriteLine("[ResetPassword] Validation failed: Missing token or new password.");
                return BadRequest(new { Message = "Token and new password are required." });
            }

            try
            {
                var result = _loginService.ResetPassword(request.Token, request.NewPassword);
                if (!result)
                {
                    Console.WriteLine("[ResetPassword] Invalid or expired token.");
                    return BadRequest(new { Message = "Invalid or expired token." });
                }

                Console.WriteLine("[ResetPassword] Password reset successful.");
                return Ok(new { Message = "Password successfully changed." });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[ResetPassword] Error: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}