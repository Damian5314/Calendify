using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;
using StarterKit.Models;
using Microsoft.EntityFrameworkCore;

namespace StarterKit.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventStorage _eventStorage;
        private readonly DatabaseContext _context;

        public EventController(IEventStorage eventStorage, DatabaseContext context)
        {
            _eventStorage = eventStorage;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _eventStorage.ReadEvents());
        }

        [HttpGet("{eventId:int}")]
        public async Task<IActionResult> GetEventById(int eventId)
        {
            var eventDetails = await _eventStorage.GetEventById(eventId);
            if (eventDetails == null)
            {
                return NotFound(new { message = "Event not found" });
            }
            return Ok(eventDetails);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event e)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _eventStorage.CreateEvent(e);
            return Ok("Event has been created");
        }

        [HttpDelete("{eventId:int}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            bool isDeleted = await _eventStorage.DeleteEvent(eventId);
            return isDeleted ? Ok($"Event with id {eventId} deleted") : NotFound("Event not found.");
        }

        [HttpPut("{eventId:int}")]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromBody] Event e)
        {
            bool isUpdated = await _eventStorage.Put(eventId, e);
            return isUpdated ? Ok($"Event with id {eventId} updated") : NotFound("Event not found.");
        }

        // 🔹 Filter evenementen op basis van recuringDays van de gebruiker
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserEvents(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var recuringDays = user.RecuringDays.Split(',')
                .Select(d => d.Trim().ToLower()) // 🔹 Zorg ervoor dat alles lowercase is
                .ToList();

            var events = await _eventStorage.ReadEvents();

            var userEvents = events
                .Where(e => recuringDays.Contains(e.EventDate.ToDateTime(TimeOnly.MinValue).DayOfWeek.ToString().ToLower()))
                .ToList();

            return Ok(userEvents);
        }
    }
}
