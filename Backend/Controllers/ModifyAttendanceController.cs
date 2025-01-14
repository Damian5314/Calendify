using Microsoft.AspNetCore.Mvc; // Importeert de basisfunctionaliteit voor het werken met controllers en het teruggeven van HTTP-responses
using Microsoft.AspNetCore.Authorization; // Importeert functionaliteit voor autorisatie om toegang te beperken
using StarterKit.Models; // Importeert de modellen zoals User, Event, Attendance voor gegevensrepresentatie
using StarterKit.Services; // Importeert services zoals ILoginService voor login-functionaliteit

namespace StarterKit.Controllers
{
    [Route("api/v1/modify-attendance")] // Geeft het basispad voor de routes in deze controller aan
    [ApiController] // Markeert deze klasse als een API-controller met automatische responsvalidatie
    public class ModifyAttendanceController : ControllerBase
    {
        // Databasecontext om toegang te krijgen tot de database, en ILoginService voor inlogbeheer
        private readonly DatabaseContext _context;
        private readonly ILoginService _loginService;

        // Constructor om de DatabaseContext en ILoginService te injecteren
        public ModifyAttendanceController(DatabaseContext context, ILoginService loginService)
        {
            _context = context; // Maakt DatabaseContext beschikbaar in de controller
            _loginService = loginService; // Maakt ILoginService beschikbaar in de controller
        }

        // Methode om alle geboekte aanwezigheden van een gebruiker op te halen
        [HttpGet("booked")]
        public IActionResult GetBookedAttendance([FromQuery] int userId)
        {
            // 1. Controleert of de gebruiker is ingelogd door UserId in de sessie te controleren
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to view your attendance.");

            // 2. Optioneel: Controleert of de ingelogde gebruiker overeenkomt met de opgegeven userId
            if (loggedInUserId != userId)
                return Forbid("You can only view your own attendance.");

            // 3. Haalt de aanwezigheidsdata voor de opgegeven userId op
            var userAttendance = _context.Attendance
                .Where(a => a.UserId == userId)
                .ToList();

            // 4. Geeft de aanwezigheidsdata terug als deze bestaat, anders 404 NotFound
            if (userAttendance.Count == 0)
                return NotFound("No attendance found for this user.");

            return Ok(userAttendance); // Retourneert de lijst van geboekte aanwezigheden met een 200 OK
        }

        // Methode om aanwezigheid te boeken
        [HttpPost("book")]
        public IActionResult BookAttendance([FromBody] BookAttendanceRequest request)
        {
            // 1. Controleert of de gebruiker is ingelogd
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to book attendance.");

            // 2. Controleert of de ingelogde gebruiker overeenkomt met UserId in het verzoek
            if (loggedInUserId != request.UserId)
                return Forbid("You can only book your own attendance.");

            // 3. Controleert of er al een boeking is op dezelfde datum en tijd
            var existingAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.AttendanceDate);

            if (existingAttendance != null)
                return Conflict("You already have a booking for this date and time."); // Geeft 409 Conflict als er al een boeking is

            // 4. Maakt een nieuwe aanwezigheid aan en voegt deze toe aan de database
            var attendance = new Attendance
            {
                UserId = request.UserId,
                AttendanceDate = request.AttendanceDate
            };

            _context.Attendance.Add(attendance);
            _context.SaveChanges();

            return Ok("Attendance successfully booked."); // Bevestigt succesvolle boeking met 200 OK
        }

        // Methode om aanwezigheid bij te werken
        [HttpPut("update")]
        public IActionResult UpdateAttendance([FromBody] UpdateAttendanceRequest request)
        {
            // 1. Controleert of de gebruiker is ingelogd
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to update attendance.");

            // 2. Controleert of de ingelogde gebruiker overeenkomt met UserId in het verzoek
            if (loggedInUserId != request.UserId)
                return Forbid("You can only update your own attendance.");

            // 3. Zoekt de bestaande aanwezigheid op de oude datum om te updaten
            var existingAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.OldAttendanceDate);

            if (existingAttendance == null)
                return NotFound("No existing booking found for the old date and time."); // 404 als oude boeking niet gevonden wordt

            // 4. Controleert of de nieuwe datum al een boeking heeft
            var conflictAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.NewAttendanceDate);

            if (conflictAttendance != null)
                return Conflict("You already have a booking for the new date and time."); // 409 Conflict bij dubbele boeking op nieuwe datum

            // 5. Werk de aanwezigheid bij met de nieuwe datum en sla op
            existingAttendance.AttendanceDate = request.NewAttendanceDate;
            _context.SaveChanges();

            return Ok("Attendance successfully updated."); // Retourneert succesbericht met 200 OK
        }

        // Methode om een geboekte aanwezigheid te verwijderen
        [HttpDelete("delete")]
        public IActionResult DeleteAttendance([FromBody] DeleteAttendanceRequest request)
        {
            // 1. Controleert of de gebruiker is ingelogd
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUserId == null)
                return Unauthorized("You need to be logged in to delete attendance.");

            // 2. Controleert of de ingelogde gebruiker overeenkomt met UserId in het verzoek
            if (loggedInUserId != request.UserId)
                return Forbid("You can only delete your own attendance.");

            // 3. Zoekt en verwijdert de aanwezigheid op de specifieke datum
            var attendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == request.UserId && a.AttendanceDate == request.AttendanceDate);

            if (attendance == null)
                return NotFound("No booking found on this date and time."); // 404 als geen boeking gevonden wordt

            _context.Attendance.Remove(attendance);
            _context.SaveChanges();

            return Ok("Attendance successfully deleted."); // Bevestigt verwijdering met 200 OK
        }
    }

    // Model voor aanwezigheid boeken
    public class BookAttendanceRequest
    {
        public int UserId { get; set; } // Gebruikers-ID voor wie de aanwezigheid wordt geboekt
        public DateTime AttendanceDate { get; set; } // Datum en tijd van de aanwezigheid
    }

    // Model voor aanwezigheid bijwerken
    public class UpdateAttendanceRequest
    {
        public int UserId { get; set; } // Gebruikers-ID voor wie de aanwezigheid wordt bijgewerkt
        public DateTime OldAttendanceDate { get; set; } // De oude datum en tijd van de aanwezigheid
        public DateTime NewAttendanceDate { get; set; } // De nieuwe datum en tijd van de aanwezigheid
    }

    // Model voor aanwezigheid verwijderen
    public class DeleteAttendanceRequest
    {
        public int UserId { get; set; } // Gebruikers-ID voor wie de aanwezigheid wordt verwijderd
        public DateTime AttendanceDate { get; set; } // Datum en tijd van de aanwezigheid die wordt verwijderd
    }
}
