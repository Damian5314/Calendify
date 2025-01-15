using Microsoft.AspNetCore.Mvc;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Controllers
{
    [Route("api/v1/attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        // Database context voor het uitvoeren van databaseoperaties
        private readonly DatabaseContext _context;

        // Constructor die de databasecontext initialiseert voor deze controller
        public AttendanceController(DatabaseContext context)
        {
            _context = context; // Zet de databasecontext vast voor gebruik binnen deze klasse
        }

        // POST: api/v1/attendance
        // Methode om een gebruiker aan een evenement toe te voegen
        public IActionResult AttendEvent([FromBody] AttendEventBody attendEventBody)
        {
            // Zoek de gebruiker op basis van de UserId die is doorgegeven in de request-body
            var user = _context.User.Find(attendEventBody.UserId);

            // Zoek het evenement op basis van de EventId die is doorgegeven in de request-body
            var eventItem = _context.Event.Find(attendEventBody.EventId);

            // Controleer of zowel de gebruiker als het evenement bestaan
            if (user == null || eventItem == null)
            {
                // Geef een foutmelding terug als gebruiker of evenement niet bestaat
                return BadRequest("Invalid user or event.");
            }

            // Haal de huidige datum en tijd op
            var now = DateTime.Now;

            // Converteer de starttijd van het evenement naar een TimeOnly object
            var eventStartTime = TimeOnly.FromTimeSpan(eventItem.StartTime);

            // Controleer of het evenement nog niet is begonnen of al voorbij is
            if (eventItem.EventDate.ToDateTime(eventStartTime) <= now)
            {
                // Geef een foutmelding terug als het evenement al gestart of voorbij is
                return BadRequest("Event has already started or passed.");
            }

            // Maak een nieuw Event_Attendance-object om de gebruiker aan het evenement toe te voegen
            var eventAttendance = new Event_Attendance
            {
                User = user,         // Verwijs naar de gebruiker
                Event = eventItem,   // Verwijs naar het evenement
                Feedback = "",       // Initialiseer zonder feedback
                Rating = 0           // Initialiseer met een rating van 0
            };

            // Voeg de aanwezigheid van de gebruiker toe aan de database
            _context.Event_Attendance.Add(eventAttendance);

            // Sla de wijzigingen op in de database
            _context.SaveChanges();

            // Stuur een succesbericht terug met de details van het evenement
            return Ok("User added to Event.");
        }

        // GET: api/v1/attendance/event/{eventId}
        // Methode om een lijst van aanwezigen voor een specifiek evenement op te halen
        [HttpGet("event/{eventId}")]
        public IActionResult GetAttendees(int eventId)
        {
            // Zoek het evenement in de database op basis van eventId
            var eventItem = _context.Event.Find(eventId);

            // Controleer of het evenement bestaat
            if (eventItem == null)
            {
                // Geef een foutmelding terug als het evenement niet gevonden wordt
                return NotFound("Event not found.");
            }

            // Haal een lijst van aanwezigen op voor het evenement, inclusief voor- en achternaam
            var attendees = _context.Event_Attendance
                .Where(ea => ea.Event.EventId == eventId)
                .Select(ea => new { ea.User.FirstName, ea.User.LastName })
                .ToList();

            // Stuur de lijst van aanwezigen terug als reactie
            return Ok(attendees);
        }

        // DELETE: api/v1/attendance
        // Methode om een gebruiker van de aanwezigheidslijst van een evenement te verwijderen
        [HttpDelete]
        public IActionResult DeleteAttendance([FromBody] AttendEventBody attendEventBody)
        {
            // Zoek het aanwezigheidsrecord voor de gebruiker en het evenement
            var attendance = _context.Event_Attendance
                .FirstOrDefault(ea => ea.Event.EventId == attendEventBody.EventId && ea.User.UserId == attendEventBody.UserId);

            // Controleer of het aanwezigheidsrecord bestaat
            if (attendance == null)
            {
                // Geef een foutmelding terug als de aanwezigheid niet gevonden is
                return NotFound("Attendance not found.");
            }

            // Verwijder het aanwezigheidsrecord uit de database
            _context.Event_Attendance.Remove(attendance);

            // Sla de wijzigingen op in de database
            _context.SaveChanges();

            // Stuur een succesbericht terug om te bevestigen dat de aanwezigheid is verwijderd
            return Ok("Attendance deleted.");
        }

        [HttpPost("feedback")]
        public IActionResult SubmitFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            // Find the attendance record
            var attendance = _context.Event_Attendance
                .FirstOrDefault(ea => ea.EventId == feedbackRequest.EventId && ea.UserId == feedbackRequest.UserId);

            if (attendance == null)
            {
                return NotFound("Attendance record not found.");
            }

            // Update feedback and rating
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

        // Define the request model for feedback
        public class FeedbackRequest
        {
            public int UserId { get; set; }
            public int EventId { get; set; }
            public string Feedback { get; set; } = string.Empty;
            public int Rating { get; set; } // e.g., 1 to 5
        }
    }

    // Definieer de klasse AttendEventBody, die de gegevensstructuur voor het verzoek beschrijft
    public class AttendEventBody
    {
        // ID van de gebruiker die aan het evenement wil deelnemen
        public int UserId { get; set; }

        // ID van het evenement waaraan de gebruiker wil deelnemen
        public int EventId { get; set; }
    }
}
