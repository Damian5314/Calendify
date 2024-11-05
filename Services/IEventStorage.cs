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

    public class DbEventStorage : IEventStorage
    {
        private readonly DatabaseContext _context;

        public DbEventStorage(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> ReadEvents()
        {
            return await _context.Event.Include(e => e.Event_Attendances).ToListAsync(); // Include related data if needed
        }

        public async Task CreateEvent(Event e)
        {
            _context.Event.Add(e);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteEvent(int eventId)
        {
            var eventToRemove = await _context.Event.FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventToRemove != null)
            {
                _context.Event.Remove(eventToRemove);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Put(int eventId, Event updatedEvent)
        {
            var existingEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventId == eventId);

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
