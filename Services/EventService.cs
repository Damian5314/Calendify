using StarterKit.Models;
using StarterKit.Utils;

namespace StarterKit.Services;

public class EventService : IEventService
{
    private readonly DatabaseContext _context;

    public EventService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _context.Events
            .Include(e => e.Reviews)
            .Include(e => e.Attendees)
            .ToListAsync();
    }
}
