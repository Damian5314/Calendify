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
        /// Handles user registration
        /// </summary>
        /// <param name="user">User details for registration</param>
        /// <returns>Status of registration</returns>
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Set default values for missing properties
            user.Role ??= "User";
            user.Attendances ??= new List<Attendance>();
            user.Event_Attendances ??= new List<Event_Attendance>();

            // Call the service to register the user
            var success = _loginService.RegisterUser(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password,
                user.RecuringDays,
                user.Role);

            if (!success)
                return Conflict("A user with this email already exists.");

            return Ok("User registered successfully.");
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var status = _loginService.CheckPassword(loginRequest.Email, loginRequest.Password);

            return status switch
            {
                LoginStatus.IncorrectEmail => Unauthorized("Email does not exist."),
                LoginStatus.IncorrectPassword => Unauthorized("Incorrect password."),
                LoginStatus.Success => Ok("Login successful."),
                _ => StatusCode(500, "An unknown error occurred."),
            };
        }
    }
}
