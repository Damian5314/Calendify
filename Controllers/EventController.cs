using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Models;
using StarterKit.Services;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly ILoginService _loginService;

    public EventController(DatabaseContext context, ILoginService loginService)
    {
        _context = context;
        _loginService = loginService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetEvents()
    {
        var events = _context.Events
            .ToList();
        return Ok(events);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateEvent([FromBody] Event eventModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //_context.Events.Add(eventModel);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetEvents), new { id = eventModel.Id }, eventModel);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateEvent(int id, [FromBody] Event eventModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingEvent = _context.Events.Find(id);
        if (existingEvent == null)
        {
            return NotFound();
        }

        existingEvent.Title = eventModel.Title;
        existingEvent.Description = eventModel.Description;
        //existingEvent.Date = eventModel.Date;
        existingEvent.StartTime = eventModel.StartTime;
        existingEvent.EndTime = eventModel.EndTime;
        existingEvent.Location = eventModel.Location;
        existingEvent.AdminApproval = eventModel.AdminApproval;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteEvent(int id)
    {
        var existingEvent = _context.Events.Find(id);
        if (existingEvent == null)
        {
            return NotFound();
        }

        _context.Events.Remove(existingEvent);
        _context.SaveChanges();
        return NoContent();
    }
}
