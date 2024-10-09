public class Event
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Location { get; set; }

    // Relationships
    public List<Review> Reviews { get; set; } = new List<Review>();
    public List<Attendee> Attendees { get; set; } = new List<Attendee>();
}
