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
            _context = context; // Zet de databasecontext vast voor gebruik binnen deze klasse
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

            var now = DateTime.Now;

            var eventStartTime = TimeOnly.FromTimeSpan(eventItem.StartTime);

            if (eventItem.EventDate.ToDateTime(eventStartTime) <= now)
            {
                // Geef een foutmelding terug als het evenement al gestart of voorbij is
                return BadRequest("Event has already started or passed.");
            }

            var eventAttendance = new Event_Attendance
            {
                User = user,         // Verwijs naar de gebruiker
                Event = eventItem,   // Verwijs naar het evenement
                Feedback = "",       // Initialiseer zonder feedback
                Rating = 0           // Initialiseer met een rating van 0
            };

            _context.Event_Attendance.Add(eventAttendance);

            _context.SaveChanges();

            return Ok("User added to Event.");
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

        // Get all events that user with userId is attending
        // GET: api/v1/attendance/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetEventsByUser(int userId)
        {
            var user = _context.User.Find(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var events = _context.Event_Attendance
                .Where(ea => ea.UserId == userId)               // Filter by userId
                .Select(ea => new                               // Select relevant event details
                {
                    ea.Event.EventId,
                    ea.Event.Title,
                    ea.Event.EventDate,
                    ea.Event.StartTime,
                    ea.Event.EndTime
                })
                .ToList();

            return Ok(events);
        }

        [HttpGet("event/{eventId}/is-attending")]
        public IActionResult IsUserAttending(int eventId, [FromQuery] int userId)
        {
            var attendanceExists = _context.Event_Attendance
                .Any(ea => ea.EventId == eventId && ea.UserId == userId);

            return Ok(new { IsAttending = attendanceExists });
        }

        [HttpPost("feedback")]
        public IActionResult SubmitFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            var attendance = _context.Event_Attendance
                .FirstOrDefault(ea => ea.EventId == feedbackRequest.EventId && ea.UserId == feedbackRequest.UserId);

            if (attendance == null)
            {
                return NotFound("Attendance record not found.");
            }

            attendance.Feedback = feedbackRequest.Feedback;
            attendance.Rating = feedbackRequest.Rating;

            _context.SaveChanges();

            return Ok("Feedback submitted successfully.");
        }

        [HttpGet("event/{eventId}/average-rating")]
        public IActionResult GetAverageRating(int eventId)
        {
            var ratings = _context.Event_Attendance
                .Where(ea => ea.EventId == eventId && ea.Rating > 0)
                .Select(ea => ea.Rating)
                .ToList();

            if (!ratings.Any())
            {
                return Ok(new { EventId = eventId, AverageRating = 0.0 });
            }

            var averageRating = ratings.Average();
            return Ok(new { EventId = eventId, AverageRating = averageRating });
        }

        [HttpGet("event/{eventId}/has-rated")]
        public IActionResult HassUserRated(int eventId, [FromQuery] int userId)
        {
            var ratingExists = _context.Event_Attendance
                .Any(ea => ea.EventId == eventId && ea.UserId == userId && ea.Rating != 0);

            return Ok(new { HasRated = ratingExists });
        }
    }

    public class FeedbackRequest
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public int Rating { get; set; } // e.g., 1 to 5
    }

    public class AttendEventBody
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
