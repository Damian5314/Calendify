using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StarterKit.Models;

[ApiController]
[Route("api/[controller]")]
public class EventController : Controller
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // GET: api/event
    [HttpGet]
    [AllowAnonymous] // Public access for viewing events
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    // POST: api/event
    [HttpPost]
    [Authorize(Roles = "Admin")] // Restricted to admin users
    public async Task<ActionResult<Event>> CreateEvent([FromBody] EventCreateDto eventDto)
    {
        var createdEvent = await _eventService.CreateEventAsync(eventDto);
        return CreatedAtAction(nameof(GetAllEvents), new { id = createdEvent.Id }, createdEvent);
    }

    // PUT, DELETE methods (as shown earlier) would also go here.
}
