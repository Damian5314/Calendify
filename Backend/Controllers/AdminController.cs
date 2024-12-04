using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Models;

namespace StarterKit.Controllers
{
    [Route("api/v1/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AdminController(DatabaseContext context)
        {
            _context = context;
        }

        // Get all users
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _context.User
                .Include(u => u.Attendances)
                .Include(u => u.Event_Attendances)
                .ThenInclude(ea => ea.Event)
                .ToList();

            return Ok(users);
        }

        // Get a specific user by ID
        [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
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

        // Create a new user
        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return Ok("User created successfully.");
        }

        // Update a user
        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _context.User.Find(id);
            if (user == null)
                return NotFound("User not found.");

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            user.Role = updatedUser.Role;
            user.RecuringDays = updatedUser.RecuringDays;

            _context.SaveChanges();
            return Ok("User updated successfully.");
        }

        // Delete a user
        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
                return NotFound("User not found.");

            _context.User.Remove(user);
            _context.SaveChanges();
            return Ok("User deleted successfully.");
        }
    }
}
