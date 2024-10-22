using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Models;

namespace StarterKit.Services
{
    public interface IEventStorage
    {
        Task<List<Event>> ReadEvents();
        Task CreateEvent(Event e);
        Task<bool> DeleteEvent(int eventId);  // Changed from Guid to int
        Task<bool> Put(int eventId, Event e); // Changed from Guid to int
    }

    public class JsonEventStorage : IEventStorage
    {
        public string path = "Data/events.json";

        public async Task<List<Event>> ReadEvents()
        {
            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path);
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonSerializer.Deserialize<List<Event>>(json) ?? new List<Event>();
                }
            }
            return new List<Event>(); // Default to empty list if file doesn't exist or is empty
        }

        public async Task CreateEvent(Event e)
        {
            // Read the current list of events from the file
            var events = await ReadEvents();

            // You can generate a unique EventId here, or let the client provide it.
            e.EventId = events.Any() ? events.Max(ev => ev.EventId) + 1 : 1;

            // Add the new event
            events.Add(e);

            // Serialize and write the updated list back to the file
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true }));
        }

        public async Task<bool> DeleteEvent(int eventId)
        {
            var events = await ReadEvents();
            var eventToDelete = events.FirstOrDefault(e => e.EventId == eventId);

            if (eventToDelete != null)
            {
                events.Remove(eventToDelete);
                await File.WriteAllTextAsync(path, JsonSerializer.Serialize(events)); // Save changes
                return true;
            }

            return false;
        }

        public async Task<bool> Put(int eventId, Event updatedEvent)
        {
            var events = await ReadEvents();
            var existingEvent = events.FirstOrDefault(e => e.EventId == eventId);

            if (existingEvent != null)
            {
                var index = events.IndexOf(existingEvent);
                events[index] = updatedEvent; // Replace the event
                await File.WriteAllTextAsync(path, JsonSerializer.Serialize(events)); // Save changes
                return true;
            }

            return false;
        }
    }

    public class DbEventStorage : IEventStorage
    {
        private readonly DatabaseContext _context;

        public DbEventStorage(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> ReadEvents()
        {
            return await _context.Events.Include(e => e.Event_Attendances).ToListAsync(); // Include related data if needed
        }

        public async Task CreateEvent(Event e)
        {
            _context.Events.Add(e);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteEvent(int eventId)
        {
            var eventToRemove = await _context.Events.FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventToRemove != null)
            {
                _context.Events.Remove(eventToRemove);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Put(int eventId, Event updatedEvent)
        {
            var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == eventId);

            if (existingEvent != null)
            {
                _context.Entry(existingEvent).CurrentValues.SetValues(updatedEvent); // Update existing entity
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
