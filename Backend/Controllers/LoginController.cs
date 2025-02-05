using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StarterKit.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public LoginController(ILoginService loginService, IConfiguration configuration, DatabaseContext context)
        {
            _loginService = loginService;
            _configuration = configuration;
            _context = context;
        }

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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { Message = "Email and Password are required." });
            }

            var (isAdmin, role) = _loginService.AdminLogin(loginRequest.Email, loginRequest.Password);
            var (userStatus, userRole) = _loginService.CheckPassword(loginRequest.Email, loginRequest.Password);

            if (isAdmin || userStatus == LoginStatus.Success)
            {
                var (userId, firstName) = isAdmin 
                    ? (_loginService.GetAdminIdByEmail(loginRequest.Email), _loginService.GetUserNameByEmailAdmin(loginRequest.Email))
                    : (_loginService.GetUserIdByEmail(loginRequest.Email), _loginService.GetFirstNameByEmail(loginRequest.Email));

                // Find the correct user type
                object userObject = isAdmin 
                    ? _context.Admin.FirstOrDefault(a => a.Email == loginRequest.Email) 
                    : _context.User.FirstOrDefault(u => u.Email == loginRequest.Email);

                if (userObject == null)
                    return NotFound(new { Message = "User not found." });

                string refreshToken = GenerateRefreshToken();
                
                // Update the refresh token based on user type
                if (isAdmin)
                {
                    Admin admin = (Admin)userObject;
                    admin.RefreshToken = refreshToken;
                    admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                }
                else
                {
                    User user = (User)userObject;
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                }
                _context.SaveChanges();

                SetRefreshTokenCookie(refreshToken);

                HttpContext.Session.SetString("Role", isAdmin ? role : userRole);

                return Ok(new { Message = "Login successful.", Role = isAdmin ? role : userRole, UserId = userId, FirstName = firstName });
            }

            return Unauthorized(new { Message = "Invalid email or password." });
        }

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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Console.WriteLine("[Logout] Logging out user.");
            HttpContext.Session.Clear();
            Response.Cookies.Delete("refreshToken");

            return Ok(new { Message = "Logged out successfully." });
        }

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

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized(new { Message = "No refresh token provided." });

            var user = _context.User.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                // If user is not found in User table, check in Admin table
                var admin = _context.Admin.FirstOrDefault(a => a.RefreshToken == refreshToken);
                if (admin == null || admin.RefreshTokenExpiry < DateTime.UtcNow)
                    return Unauthorized(new { Message = "Invalid or expired refresh token." });

                HttpContext.Session.SetInt32("UserId", admin.AdminId);
                HttpContext.Session.SetString("FirstName", admin.UserName);
                HttpContext.Session.SetString("Role", "Admin");

                return Ok(new { Message = "Session restored.", UserId = admin.AdminId, FirstName = admin.UserName, Role = "Admin" });
            }
            else if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized(new { Message = "Invalid or expired refresh token." });
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("Role", user.Role);

            return Ok(new { Message = "Session restored.", UserId = user.UserId, FirstName = user.FirstName, Role = user.Role });
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private void SetRefreshTokenCookie(string token)
        {
            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}