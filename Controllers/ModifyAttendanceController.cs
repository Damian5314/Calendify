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

        // POST: api/v1/modify-attendance/book
        [HttpPost("book")]
        public IActionResult BookAttendance([FromBody] BookAttendanceRequest request)
        {
            // 1. Controleer of gebruiker is ingelogd
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("Je moet ingelogd zijn om aanwezigheid te boeken.");

            // 2. Controleer of de gebruiker zijn eigen boeking probeert te maken
            if (loggedInUserId != request.UserId)
                return Forbid("Je kunt alleen je eigen aanwezigheid boeken.");

            // 3. Controleer of er al een boeking is op de datum
            var existingAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.AttendanceDate);


            if (existingAttendance != null)
                return Conflict("Je hebt al een boeking voor deze datum.");

            // 4. Maak een nieuwe aanwezigheid (attendance) aan
            var attendance = new Attendance
            {
                UserId = request.UserId,
                AttendanceDate = request.AttendanceDate
            };

            _context.Attendance.Add(attendance);
            _context.SaveChanges();

            return Ok("Aanwezigheid succesvol geboekt.");
        }

        // PUT: api/v1/modify-attendance/update
        [HttpPut("update")]
        public IActionResult UpdateAttendance([FromBody] UpdateAttendanceRequest request)
        {
            // 1. Controleer of gebruiker is ingelogd
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("Je moet ingelogd zijn om aanwezigheid te wijzigen.");

            // 2. Controleer of de gebruiker zijn eigen boeking probeert te updaten
            if (loggedInUserId != request.UserId)
                return Forbid("Je kunt alleen je eigen aanwezigheid wijzigen.");

            // 3. Vind de bestaande aanwezigheid
            var existingAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate.Date == request.OldAttendanceDate.Date);

            if (existingAttendance == null)
                return NotFound("Geen bestaande boeking gevonden op deze datum.");

            // 4. Controleer of de nieuwe datum al geboekt is
            var conflictAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate.Date == request.NewAttendanceDate.Date);

            if (conflictAttendance != null)
                return Conflict("Je hebt al een boeking voor de nieuwe datum.");

            // 5. Wijzig de datum en sla op
            existingAttendance.AttendanceDate = request.NewAttendanceDate;
            _context.SaveChanges();

            return Ok("Aanwezigheid succesvol gewijzigd.");
        }

        // DELETE: api/v1/modify-attendance/delete
        [HttpDelete("delete")]
        public IActionResult DeleteAttendance([FromBody] DeleteAttendanceRequest request)
        {
            // 1. Controleer of gebruiker is ingelogd
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("Je moet ingelogd zijn om aanwezigheid te verwijderen.");

            // 2. Controleer of de gebruiker zijn eigen boeking probeert te verwijderen
            if (loggedInUserId != request.UserId)
                return Forbid("Je kunt alleen je eigen aanwezigheid verwijderen.");

            // 3. Zoek de aanwezigheid en verwijder deze
            var attendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate.Date == request.AttendanceDate.Date);

            if (attendance == null)
                return NotFound("Geen boeking gevonden op deze datum.");

            _context.Attendance.Remove(attendance);
            _context.SaveChanges();

            return Ok("Aanwezigheid succesvol verwijderd.");
        }
    }

    // Request modellen voor data validatie
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
