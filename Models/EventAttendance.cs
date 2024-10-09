using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class EventAttendance
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    
    [ForeignKey("Event")]
    public int EventId { get; set; }

    public string? Rating { get; set; }

    public string? Feedback { get; set; }

    // Navigation properties
    public required User User { get; set; }
    public required Event Event { get; set; }
}
