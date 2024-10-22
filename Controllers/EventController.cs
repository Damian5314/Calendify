using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;
using StarterKit.Models;

namespace StarterKit.Controllers
{
    [Route("api/v1/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventStorage _eventStorage;

        public EventController(IEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

        // Users can access this
        [HttpGet]
        [Authorize] // Allow any authenticated user (both admins and regular users)
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _eventStorage.ReadEvents());
        }

        // Only Admins can create/delete/update events
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent([FromBody] Event e)
        {
            await _eventStorage.CreateEvent(e);
            return Ok("Event has been created");
        }

        [HttpDelete("{eventId:int}")]  // Changed from Guid to int
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int eventId)  // Changed from Guid to int
        {
            return await _eventStorage.DeleteEvent(eventId) 
                ? Ok($"Event with id {eventId} deleted") 
                : NotFound("Event with that Id doesn't exist.");
        }

        [HttpPut("{eventId:int}")]  // Changed from Guid to int
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent([FromRoute] int eventId, [FromBody] Event e)  // Changed from Guid to int
        {
            return await _eventStorage.Put(eventId, e) 
                ? Ok($"Event with id {eventId} updated") 
                : NotFound("Event with that Id doesn't exist.");
        }
    }


    //Deze komt uit Models.Calender.cs maar moet hier plakken want anders die het niet vinden.
}