using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;
using StarterKit.Models;

namespace StarterKit.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventStorage _eventStorage;

        public EventController(IEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

       
        [HttpGet]
       
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _eventStorage.ReadEvents());
        }

        // alleenn admins kan create/delete/update events
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent([FromBody] Event e)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // dit geeft foutmelding 400
            }
            await _eventStorage.CreateEvent(e);
            return Ok("Event has been created");
        }

        [HttpDelete("{eventId:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int eventId)
        {
            return await _eventStorage.DeleteEvent(eventId) 
                ? Ok($"Event with id {eventId} deleted") 
                : NotFound("Event with that Id doesn't exist.");
        }

        [HttpPut("{eventId:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent([FromRoute] int eventId, [FromBody] Event e)
        {
            return await _eventStorage.Put(eventId, e) 
                ? Ok($"Event with id {eventId} updated") 
                : NotFound("Event with that Id doesn't exist.");
        }
    }
}