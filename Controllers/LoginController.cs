using System.Text;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;

namespace StarterKit.Controllers
{
    [Route("api/v1/Login")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginBody loginBody)
        {
            var loginState = _loginService.CheckPassword(loginBody.Email!, loginBody.Password!);

            switch (loginState)
            {
                case LoginStatus.Success:
                    var userId = _loginService.GetUserIdByEmail(loginBody.Email); // Ensure this method exists to get user ID
                    HttpContext.Session.SetInt32("UserId", userId); // Store UserId in session
                    HttpContext.Session.SetString("AdminSession", "LoggedIn");
                    HttpContext.Session.SetString("LoggedInAdmin", $"{loginBody.Email}");
                    HttpContext.Session.SetString("LoggedInUser", $"{loginBody.Email}");
                    return Ok($"Hello {loginBody.Email}, login success");

                case LoginStatus.IncorrectPassword:
                    return Unauthorized("Incorrect password");

                case LoginStatus.IncorrectEmail:
                    return Unauthorized("Incorrect email");

                default:
                    return BadRequest();
            }
        }



        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterBody registerBody)
        {
            if (registerBody == null)
            {
                return BadRequest("Request body is null.");
            }

            if (string.IsNullOrEmpty(registerBody.FirstName))
            {
                return BadRequest("First name is required.");
            }

            if (string.IsNullOrEmpty(registerBody.LastName))
            {
                return BadRequest("Last name is required.");
            }

            if (string.IsNullOrEmpty(registerBody.Email))
            {
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrEmpty(registerBody.Password))
            {
                return BadRequest("Password is required.");
            }

            if (string.IsNullOrEmpty(registerBody.RecuringDays))
            {
                return BadRequest("Recurring days are required.");
            }

            var isRegistered = _loginService.RegisterUser(
                registerBody.FirstName,
                registerBody.LastName,
                registerBody.Email,
                registerBody.Password,
                registerBody.RecuringDays
            );

            if (isRegistered)
            {
                return Ok("User registration successful.");
            }

            return BadRequest("User already exists.");
        }

        [HttpGet("IsAdminLoggedIn")]
        public IActionResult IsAdminLoggedIn()
        {
            if (IsSessionRegistered()) 
                return Ok($"Logged in as {HttpContext.Session.GetString("LoggedInAdmin")}");
            return Unauthorized("You are not logged in as Admin");
        }

        [HttpGet("IsUserLoggedIn")]
        public IActionResult IsUserLoggedIn()
        {
            if (IsSessionRegistered()) 
                return Ok($"Logged in as {HttpContext.Session.GetString("LoggedInUser")}");
            return Unauthorized("You are not logged in");
        }


        [HttpGet("IsSessionRegistered")]
        public bool IsSessionRegistered()
        {
            return HttpContext.Session.GetString("AdminSession") == "LoggedIn";
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            if (IsSessionRegistered())
            {
                HttpContext.Session.SetString("AdminSession", "LoggedOut");
                return Ok("Logged out");
            }
            return BadRequest("You are not logged in");
        }
    }

    public class LoginBody
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class RegisterBody
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? RecuringDays { get; set; }
    }
}
