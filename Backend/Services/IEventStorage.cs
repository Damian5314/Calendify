using Microsoft.EntityFrameworkCore;
using StarterKit.Models;

namespace StarterKit.Services
{
    public interface IEventStorage
    {
        Task<List<Event>> ReadEvents();
        Task<List<Event>> ReadUserEvents(int userId, List<string> recuringDays);
        Task CreateEvent(Event e);
        Task<bool> DeleteEvent(int eventId);
        Task<bool> Put(int eventId, Event e);
        Task<Event?> GetEventById(int eventId);
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
            return await _context.Event.Include(e => e.Event_Attendances).ToListAsync();
        }

        public async Task<Event?> GetEventById(int eventId)
        {
            return await _context.Event
                .Include(e => e.Event_Attendances)
                .FirstOrDefaultAsync(e => e.EventId == eventId);
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
                existingEvent.Title = updatedEvent.Title;
                existingEvent.Description = updatedEvent.Description;
                existingEvent.EventDate = updatedEvent.EventDate;
                existingEvent.StartTime = updatedEvent.StartTime;
                existingEvent.EndTime = updatedEvent.EndTime;
                existingEvent.Location = updatedEvent.Location;
                existingEvent.AdminApproval = updatedEvent.AdminApproval;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Event>> ReadUserEvents(int userId, List<string> recuringDays)
        {
            var events = await _context.Event.Include(e => e.Event_Attendances).ToListAsync();

            return events
                .Where(e => recuringDays.Contains(e.EventDate.ToDateTime(TimeOnly.MinValue).DayOfWeek.ToString().ToLower()))
                .ToList();
        }
    }
}
