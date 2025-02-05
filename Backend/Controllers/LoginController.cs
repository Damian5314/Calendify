using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;
using System.Security.Claims;
using System.Threading.Tasks;

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
            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest(new { Message = "All fields (First Name, Last Name, Email, Password) are required." });
            }

            var isRegistered = _loginService.RegisterUser(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password,
                user.RecuringDays
            );

            if (!isRegistered)
            {
                return Conflict(new { Message = "A user with the given email already exists." });
            }

            return Ok(new { Message = "User registered successfully." });
        }

        // User & Admin Login (Cookie-based authentication)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { Message = "Email and Password are required." });
            }

            var (isAdmin, role) = _loginService.AdminLogin(loginRequest.Email, loginRequest.Password);
            int userId;
            string firstName;

            if (isAdmin)
            {
                userId = _loginService.GetAdminIdByEmail(loginRequest.Email);
                firstName = _loginService.GetUserNameByEmailAdmin(loginRequest.Email);
            }
            else
            {
                var status = _loginService.CheckPassword(loginRequest.Email, loginRequest.Password);
                if (status != LoginStatus.Success)
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }

                userId = _loginService.GetUserIdByEmail(loginRequest.Email);
                firstName = _loginService.GetFirstNameByEmail(loginRequest.Email);
                role = "User";
            }

            // Maak claims aan voor authenticatie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, firstName),
                new Claim(ClaimTypes.Role, role),
                new Claim("UserId", userId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Maak de login-cookie aan
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                new AuthenticationProperties { IsPersistent = true });

            // Bewaar gegevens in sessie
            HttpContext.Session.SetString("Role", role);
            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetString("FirstName", firstName);

            return Ok(new { Message = "Login successful.", Role = role, UserId = userId, FirstName = firstName });
        }

        // Check if Logged In
        [HttpGet("is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");
            var firstName = HttpContext.Session.GetString("FirstName");

            if (string.IsNullOrEmpty(role) || userId == null)
            {
                return Unauthorized(new { Message = "You are not logged in." });
            }

            return Ok(new { Message = $"Logged in as {role}", Role = role, UserId = userId, FirstName = firstName });
        }

        // Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Ok(new { Message = "Logged out successfully." });
        }

        // Forgot Password: Generate Reset Token
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { Message = "Email is required." });
            }

            var result = _loginService.GeneratePasswordResetToken(email);
            if (!result)
            {
                return NotFound(new { Message = "Email not found in the system." });
            }

            return Ok(new { Message = "A password reset link has been sent to your email." });
        }

        // Get User Profile
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

            return Ok(new { Role = role, UserId = userId, FirstName = firstName });
        }

        // Reset Password
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new { Message = "Token and new password are required." });
            }

            var result = _loginService.ResetPassword(request.Token, request.NewPassword);
            if (!result)
            {
                return BadRequest(new { Message = "Invalid or expired token." });
            }

            return Ok(new { Message = "Password successfully changed." });
        }
    }
}
