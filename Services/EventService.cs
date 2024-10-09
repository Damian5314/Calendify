using StarterKit.Models;
using StarterKit.Utils;
using Microsoft.EntityFrameworkCore;

namespace StarterKit.Services;

public class EventService
{
    private readonly DatabaseContext _context;

    public EventService(DatabaseContext context)
    {
        _context = context;
    }

    // public IEnumerable<Event> GetAllEvents()
    // {
    //     return _context.Events
    //         .Include(e => e.Attendees)
    //         .Include(e => e.Reviews)
    //         .ToList();
    // }

    public bool CreateEvent(Event eventModel)
    {
        //_context.Events.Add(eventModel);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateEvent(int eventId, Event eventModel)
    {
        var existingEvent = _context.Events.Find(eventId);
        if (existingEvent == null) return false;

        existingEvent.Title = eventModel.Title;
        existingEvent.Description = eventModel.Description;
        //existingEvent.Date = eventModel.Date;
        existingEvent.StartTime = eventModel.StartTime;
        existingEvent.EndTime = eventModel.EndTime;
        existingEvent.Location = eventModel.Location;

        return _context.SaveChanges() > 0;
    }

    public bool DeleteEvent(int eventId)
    {
        var existingEvent = _context.Events.Find(eventId);
        if (existingEvent == null) return false;

        _context.Events.Remove(existingEvent);
        return _context.SaveChanges() > 0;
    }
}