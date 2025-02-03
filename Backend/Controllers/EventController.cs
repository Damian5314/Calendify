using Microsoft.AspNetCore.Authorization; // Importeert autorisatiefunctionaliteit om toegang te beperken
using Microsoft.AspNetCore.Mvc; // Importeert de functionaliteit voor het maken van API-controllers
using StarterKit.Services; // Importeert services, waaronder IEventStorage, om met evenementen te werken
using StarterKit.Models; // Importeert modellen, waaronder Event, die de structuur van gegevens beschrijven

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

        [HttpGet] // Dit is een GET-verzoek, wat betekent dat de methode gegevens zal ophalen
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _eventStorage.ReadEvents()); // Geeft de lijst van evenementen terug met een 200 OK-status
        }

        [HttpGet("{eventId:int}")] // Dit is een GET-verzoek dat een specifieke ID accepteert in de URL
        public async Task<IActionResult> GetEventById(int eventId)
        {
            var eventDetails = await _eventStorage.GetEventById(eventId);

            if (eventDetails == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            return Ok(eventDetails);
        }

        [HttpPost] // Dit is een POST-verzoek, wat betekent dat de methode gegevens zal verzenden (om een evenement aan te maken)
        public async Task<IActionResult> CreateEvent([FromBody] Event e)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retourneert 400 Bad Request als de JSON onvolledig is
            }

            await _eventStorage.CreateEvent(e);

            return Ok("Event has been created"); // Retourneert 200 OK met een bevestigingsbericht
        }

        [HttpDelete("{eventId:int}")] // Dit is een DELETE-verzoek en verwacht een eventId in de URL als integer
        public async Task<IActionResult> DeleteEvent([FromRoute] int eventId)
        {
            bool isDeleted = await _eventStorage.DeleteEvent(eventId);

            return isDeleted 
                ? Ok($"Event with id {eventId} deleted") // Retourneert 200 OK met een bericht dat het evenement is verwijderd
                : NotFound("Event with that Id doesn't exist."); // Retourneert 404 Not Found als het evenement niet bestaat
        }

        [HttpPut("{eventId:int}")] // Dit is een PUT-verzoek en verwacht een eventId in de URL als integer
        public async Task<IActionResult> UpdateEvent([FromRoute] int eventId, [FromBody] Event e)
        {
            bool isUpdated = await _eventStorage.Put(eventId, e);

            return isUpdated 
                ? Ok($"Event with id {eventId} updated") // Retourneert 200 OK met een bericht dat het evenement is bijgewerkt
                : NotFound("Event with that Id doesn't exist."); // Retourneert 404 Not Found als het evenement niet bestaat
        }
    }
}
