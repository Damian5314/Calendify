using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Controllers
{
    [Route("api/v1/attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AttendanceController(DatabaseContext context)
        {
            _context = context;
        }

        // POST: api/v1/attendance
        public IActionResult AttendEvent([FromBody] AttendEventBody attendEventBody)
        {
            var user = _context.User.Find(attendEventBody.UserId);
            var eventItem = _context.Event.Find(attendEventBody.EventId);

            if (user == null || eventItem == null)
            {
                return BadRequest("Invalid user or event.");
            }

            // kijkt of de event beschikbaar is
            var now = DateTime.Now;

            // Convert TimeSpan to TimeOnly
            var eventStartTime = TimeOnly.FromTimeSpan(eventItem.StartTime);

            if (eventItem.EventDate.ToDateTime(eventStartTime) <= now)
            {
                return BadRequest("Event has already started or passed.");
            }

            // voegt de attendance toe
            var eventAttendance = new Event_Attendance
            {
                User = user,
                Event = eventItem,
                Feedback = "",
                Rating = 0 
            };

            _context.Event_Attendance.Add(eventAttendance);
            _context.SaveChanges();

            return Ok(eventItem);
        }

        // GET: api/v1/attendance/event/{eventId}
        [HttpGet("event/{eventId}")]
        public IActionResult GetAttendees(int eventId)
        {
            var eventItem = _context.Event.Find(eventId);
            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            var attendees = _context.Event_Attendance
                .Where(ea => ea.Event.EventId == eventId)
                .Select(ea => new { ea.User.FirstName, ea.User.LastName })
                .ToList();

            return Ok(attendees);
        }

        // DELETE: api/v1/attendance
        [HttpDelete]
        public IActionResult DeleteAttendance([FromBody] AttendEventBody attendEventBody)
        {
            var attendance = _context.Event_Attendance
                .FirstOrDefault(ea => ea.Event.EventId == attendEventBody.EventId && ea.User.UserId == attendEventBody.UserId);

            if (attendance == null)
            {
                return NotFound("Attendance not found.");
            }

            _context.Event_Attendance.Remove(attendance);
            _context.SaveChanges();

            return Ok("Attendance deleted.");
        }
    }

    public class AttendEventBody
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
