using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Event
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Title { get; set; }

    [Required]
    [StringLength(500)]
    public required string Description { get; set; }

    [Required]
    public required DateTime Date { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    [Required]
    [StringLength(200)]
    public required string Location { get; set; }

    public bool AdminApproval { get; set; }

    public required ICollection<Review> Reviews { get; set; }
    public required ICollection<EventAttendance> Attendees { get; set; }
}
