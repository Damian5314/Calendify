using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Controllers
{
    [Route("api/v1/modify-attendance")]
    [ApiController]
    public class ModifyAttendanceController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILoginService _loginService;

        public ModifyAttendanceController(DatabaseContext context, ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }
        [HttpGet("booked")]
        public IActionResult GetBookedAttendance([FromQuery] int userId)
        {
            // 1. Check if the user is logged in (optional, since you're using the userId directly)
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to view your attendance.");

            // 2. Verify that the provided userId matches the logged-in user (optional, you may want this)
            if (loggedInUserId != userId)
                return Forbid("You can only view your own attendance.");

            // 3. Fetch the attendance data for the given userId
            var userAttendance = _context.Attendance
                .Where(a => a.UserId == userId)
                .ToList();

            // 4. Return the attendance data as a response
            if (userAttendance.Count == 0)
                return NotFound("No attendance found for this user.");

            return Ok(userAttendance);
        }


        // POST: api/v1/modify-attendance/book
        [HttpPost("book")]
        public IActionResult BookAttendance([FromBody] BookAttendanceRequest request)
        {
            // 1. Check if the user is logged in
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to book attendance.");

            // 2. Check if the user is trying to book their own attendance
            if (loggedInUserId != request.UserId)
                return Forbid("You can only book your own attendance.");

            // 3. Check if there's already a booking on the specific date and time
            var existingAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.AttendanceDate);

            if (existingAttendance != null)
                return Conflict("You already have a booking for this date and time.");

            // 4. Create a new attendance record
            var attendance = new Attendance
            {
                UserId = request.UserId,
                AttendanceDate = request.AttendanceDate
            };

            _context.Attendance.Add(attendance);
            _context.SaveChanges();

            return Ok("Attendance successfully booked.");
        }

        // PUT: api/v1/modify-attendance/update
        [HttpPut("update")]
        public IActionResult UpdateAttendance([FromBody] UpdateAttendanceRequest request)
        {
            // 1. Check if the user is logged in
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to update attendance.");

            // 2. Check if the user is trying to update their own attendance
            if (loggedInUserId != request.UserId)
                return Forbid("You can only update your own attendance.");

            // 3. Find the existing attendance record by OldAttendanceDate
            var existingAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.OldAttendanceDate);

            if (existingAttendance == null)
                return NotFound("No existing booking found for the old date and time.");

            // 4. Check if the new date already has a booking
            var conflictAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.NewAttendanceDate);

            if (conflictAttendance != null)
                return Conflict("You already have a booking for the new date and time.");

            // 5. Update the attendance date and save changes
            existingAttendance.AttendanceDate = request.NewAttendanceDate;
            _context.SaveChanges();

            return Ok("Attendance successfully updated.");
        }

        // DELETE: api/v1/modify-attendance/delete
        [HttpDelete("delete")]
        public IActionResult DeleteAttendance([FromBody] DeleteAttendanceRequest request)
        {
            // 1. Check if the user is logged in
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to delete attendance.");

            // 2. Check if the user is trying to delete their own attendance
            if (loggedInUserId != request.UserId)
                return Forbid("You can only delete your own attendance.");

            // 3. Find and delete the attendance by date and time
            var attendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.AttendanceDate);

            if (attendance == null)
                return NotFound("No booking found on this date and time.");

            _context.Attendance.Remove(attendance);
            _context.SaveChanges();

            return Ok("Attendance successfully deleted.");
        }
    }

    // Request models for data validation
    public class BookAttendanceRequest
    {
        public int UserId { get; set; }
        public DateTime AttendanceDate { get; set; }
    }

    public class UpdateAttendanceRequest
    {
        public int UserId { get; set; }
        public DateTime OldAttendanceDate { get; set; }
        public DateTime NewAttendanceDate { get; set; }
    }

    public class DeleteAttendanceRequest
    {
        public int UserId { get; set; }
        public DateTime AttendanceDate { get; set; }
    }

    namespace StarterKit.Models
{
    public class User
    {
        public int UserId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        // A comma sepparated string that could look like this: "mo,tu,we,th,fr"
        public required string RecuringDays { get; set; }

        public required List<Attendance> Attendances { get; set; }

        public required List<Event_Attendance> Event_Attendances { get; set; }
    }

    public class Attendance
    {
        public int AttendanceId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public required User User { get; set; }
    }

    public class Event_Attendance
    {
        public int Event_AttendanceId { get; set; }
        public int Rating { get; set; }
        public required string Feedback { get; set; }
        public required User User { get; set; }
        public required Event Event { get; set; }
    }

    public class Event
    {
        public int EventId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public DateOnly EventDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public required string Location { get; set; }

        public bool AdminApproval { get; set; }

        public required List<Event_Attendance> Event_Attendances { get; set; }
    }
}
}
