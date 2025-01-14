using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Models;

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

        // Get user's profile
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

        // Update user's profile
        [HttpPut("profile/{id}")]
        public IActionResult UpdateProfile(int id, [FromBody] User updatedUser)
        {
            var user = _context.User.Find(id);
            if (user == null)
                return NotFound("User not found.");

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.RecuringDays = updatedUser.RecuringDays;

            _context.SaveChanges();
            return Ok("Profile updated successfully.");
        }
    }
}
