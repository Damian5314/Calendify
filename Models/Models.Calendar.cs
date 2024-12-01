namespace StarterKit.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string RecuringDays { get; set; } = string.Empty;

        // Default these to empty lists to avoid null references
        public List<Attendance> Attendances { get; set; } = new();
        public List<Event_Attendance> Event_Attendances { get; set; } = new();
    }

        public class Attendance
    {
        public int AttendanceId { get; set; }
        public DateTime AttendanceDate { get; set; }

        // Voeg deze eigenschap toe
        public int UserId { get; set; }  // Dit maakt directe toegang tot UserId mogelijk

        public User User { get; set; }
    }


    public class Event_Attendance
    {
        public int Event_AttendanceId { get; set; }

        public int Rating { get; set; }

        public required string Feedback { get; set; }

        // Foreign key for User
        public int UserId { get; set; }
        public required User User { get; set; }

        // Foreign key for Event
        public int EventId { get; set; }
        public required Event Event { get; set; }
    }

    public class Event
    {
        public int EventId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public DateOnly EventDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public required string Location { get; set; }

        public bool AdminApproval { get; set; }

        public required List<Event_Attendance> Event_Attendances { get; set; }
    }
}