using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Models;
using System.Text.Json;

namespace StarterKit.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        // 🔹 Haal ingelogde gebruiker op
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized("You are not logged in.");

            var user = _context.User
                .Include(u => u.Attendances)
                .Include(u => u.Event_Attendances)
                .ThenInclude(ea => ea.Event)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // 🔹 Haal profiel op via ID
        [HttpGet("profile/{id}")]
        public IActionResult GetProfile(int id)
        {
            var user = _context.User
                .Include(u => u.Attendances)
                .Include(u => u.Event_Attendances)
                .ThenInclude(ea => ea.Event)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // 🔹 Haal terugkerende dagen op
        [HttpGet("recurring-days/{id}")]
        public IActionResult GetRecurringDays(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user.RecuringDays);
        }

        // 🔹 Update terugkerende dagen
        [HttpPut("recurring-days/{id}")]
        public IActionResult UpdateRecurringDays(int id, [FromBody] List<string> newDays)
        {
            var user = _context.User.Find(id);
            if (user == null)
                return NotFound("User not found.");

            user.RecuringDays = newDays;
            _context.SaveChanges();

            return Ok(new { message = "Recurring days updated successfully." });
        }

        // 🔹 Update e-mail
        [HttpPut("update-email")]
        public IActionResult UpdateEmail([FromBody] UpdateEmailRequest request)
        {
            var user = _context.User.FirstOrDefault(u => u.UserId == request.UserId);
            if (user == null)
                return NotFound("User not found.");

            if (_context.User.Any(u => u.Email == request.NewEmail && u.UserId != request.UserId))
                return Conflict("Email already in use.");

            user.Email = request.NewEmail;
            _context.SaveChanges();

            return Ok(new { message = "Email updated successfully." });
        }

        // 🔹 Update wachtwoord met validatie
        [HttpPut("update-password")]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var user = _context.User.FirstOrDefault(u => u.UserId == request.UserId);
            if (user == null)
                return NotFound("User not found.");

            if (user.Password != request.CurrentPassword)
                return BadRequest("Current password is incorrect.");

            // 🔹 Extra validatie voor een sterk wachtwoord
            if (request.NewPassword.Length < 6)
                return BadRequest("New password must be at least 6 characters long.");

            user.Password = request.NewPassword;
            _context.SaveChanges();

            return Ok(new { message = "Password updated successfully." });
        }

        // 🔹 Update profiel (First Name, Last Name & Recurring Days)
        [HttpPut("profile/{id}")]
        public IActionResult UpdateProfile(int id, [FromBody] UpdateProfileRequest updatedUser)
        {
            var user = _context.User.Find(id);
            if (user == null)
                return NotFound("User not found.");

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.RecuringDays = updatedUser.RecuringDays;

            _context.SaveChanges();
            return Ok(new { message = "Profile updated successfully." });
        }
    }

    // 🔹 Request models voor updates
    public class UpdateEmailRequest
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; } = string.Empty;
    }

    public class UpdatePasswordRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> RecuringDays { get; set; } = new();
    }
}
