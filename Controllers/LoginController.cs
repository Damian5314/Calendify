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
            var loginState = _loginService.CheckPassword(loginBody.Username!, loginBody.Password!);

            switch (loginState)
            {
                case LoginStatus.Success:
                    HttpContext.Session.SetString("AdminSession", "LoggedIn");
                    HttpContext.Session.SetString("LoggedInAdmin", $"{loginBody.Username}");
                    return Ok("Admin login success");

                case LoginStatus.IncorrectPassword:
                    return Unauthorized("Incorrect password");

                case LoginStatus.IncorrectUsername:
                    return Unauthorized("Incorrect username");

                default:
                    return BadRequest();
            }
        }

        [HttpGet("IsAdminLoggedIn")]
        public IActionResult IsAdminLoggedIn()
        {
            if (IsSessionRegistered()) 
                return Ok($"Logged in as {HttpContext.Session.GetString("LoggedInAdmin")}");
            return Unauthorized("You are not logged in as Admin");
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
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
