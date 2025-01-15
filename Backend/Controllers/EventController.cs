using Microsoft.AspNetCore.Authorization; // Importeert autorisatiefunctionaliteit om toegang te beperken
using Microsoft.AspNetCore.Mvc; // Importeert de functionaliteit voor het maken van API-controllers
using StarterKit.Services; // Importeert services, waaronder IEventStorage, om met evenementen te werken
using StarterKit.Models; // Importeert modellen, waaronder Event, die de structuur van gegevens beschrijven

namespace StarterKit.Controllers
{
    // Routeert alle API-verzoeken die beginnen met "api/v1/events" naar deze controller
    [Route("api/v1/events")]
    // Geeft aan dat deze klasse een API-controller is en biedt automatische functionaliteiten zoals validatie
    [ApiController]
    public class EventController : ControllerBase
    {
        // Verwijzing naar de IEventStorage service, die nodig is voor alle bewerkingen met evenementen
        private readonly IEventStorage _eventStorage;

        // Constructor die de IEventStorage service instelt wanneer de controller wordt aangemaakt
        public EventController(IEventStorage eventStorage)
        {
            // De meegegeven eventStorage wordt opgeslagen in _eventStorage voor gebruik in de methodes
            _eventStorage = eventStorage;
        }

        // Methode om alle evenementen op te halen
        [HttpGet] // Dit is een GET-verzoek, wat betekent dat de methode gegevens zal ophalen
        //[Authorize] // Als dit was ingeschakeld, zouden alleen ingelogde gebruikers deze methode kunnen aanroepen
        public async Task<IActionResult> GetEvents()
        {
            // Roept de ReadEvents-methode aan uit de _eventStorage service om een lijst van evenementen op te halen
            return Ok(await _eventStorage.ReadEvents()); // Geeft de lijst van evenementen terug met een 200 OK-status
        }

        // Methode om een specifiek evenement op te halen op basis van zijn ID
        [HttpGet("{eventId:int}")] // Dit is een GET-verzoek dat een specifieke ID accepteert in de URL
        public async Task<IActionResult> GetEventById(int eventId)
        {
            // Roep de methode aan uit _eventStorage service om het evenement met de opgegeven ID op te halen
            var eventDetails = await _eventStorage.GetEventById(eventId);

            if (eventDetails == null)
            {
                // Retourneer een 404 Not Found als het evenement niet bestaat
                return NotFound(new { message = "Event not found" });
            }

            // Retourneer het evenement met een 200 OK-status
            return Ok(eventDetails);
        }

        // Methode om een nieuw evenement aan te maken
        [HttpPost] // Dit is een POST-verzoek, wat betekent dat de methode gegevens zal verzenden (om een evenement aan te maken)
        //[Authorize(Roles = "Admin")] // Als dit was ingeschakeld, zouden alleen gebruikers met de rol "Admin" deze methode kunnen aanroepen
        public async Task<IActionResult> CreateEvent([FromBody] Event e)
        {
            // Controleert of de gegevens geldig zijn (bijvoorbeeld of verplichte velden zijn ingevuld)
            if (!ModelState.IsValid)
            {
                // Geeft een foutmelding terug als de gegevens onvolledig of ongeldig zijn
                return BadRequest(ModelState); // Retourneert 400 Bad Request als de JSON onvolledig is
            }

            // Roept de CreateEvent-methode aan uit de _eventStorage service om het evenement op te slaan in de database
            await _eventStorage.CreateEvent(e);

            // Geeft een bevestiging terug dat het evenement succesvol is aangemaakt
            return Ok("Event has been created"); // Retourneert 200 OK met een bevestigingsbericht
        }

        // Methode om een specifiek evenement te verwijderen op basis van het eventId
        [HttpDelete("{eventId:int}")] // Dit is een DELETE-verzoek en verwacht een eventId in de URL als integer
        //[Authorize(Roles = "Admin")] // Als dit was ingeschakeld, zouden alleen Admin-gebruikers deze methode mogen uitvoeren
        public async Task<IActionResult> DeleteEvent([FromRoute] int eventId)
        {
            // Roept de DeleteEvent-methode aan uit de _eventStorage service om het evenement te verwijderen
            bool isDeleted = await _eventStorage.DeleteEvent(eventId);

            // Controleert of het evenement succesvol is verwijderd
            return isDeleted 
                ? Ok($"Event with id {eventId} deleted") // Retourneert 200 OK met een bericht dat het evenement is verwijderd
                : NotFound("Event with that Id doesn't exist."); // Retourneert 404 Not Found als het evenement niet bestaat
        }

        // Methode om een bestaand evenement bij te werken op basis van het eventId
        [HttpPut("{eventId:int}")] // Dit is een PUT-verzoek en verwacht een eventId in de URL als integer
        //[Authorize(Roles = "Admin")] // Als dit was ingeschakeld, zouden alleen Admin-gebruikers deze methode mogen uitvoeren
        public async Task<IActionResult> UpdateEvent([FromRoute] int eventId, [FromBody] Event e)
        {
            // Roept de Put-methode aan uit de _eventStorage service om het evenement bij te werken
            bool isUpdated = await _eventStorage.Put(eventId, e);

            // Controleert of het evenement succesvol is bijgewerkt
            return isUpdated 
                ? Ok($"Event with id {eventId} updated") // Retourneert 200 OK met een bericht dat het evenement is bijgewerkt
                : NotFound("Event with that Id doesn't exist."); // Retourneert 404 Not Found als het evenement niet bestaat
        }
    }
}
