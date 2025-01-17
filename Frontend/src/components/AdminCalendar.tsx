import React, { useState, useEffect } from "react";
import { Calendar, momentLocalizer } from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css"; // Import the styles
import { useNavigate } from "react-router-dom";

moment.updateLocale("en", {
  week: {
    dow: 1, // 1 = Monday
  },
});

const AdminCalendarPage: React.FC = () => {
  const localizer = momentLocalizer(moment); // Setup moment.js as the localizer
  const [events, setEvents] = useState<any[]>([]); // Store events for the calendar
  const navigate = useNavigate();

  useEffect(() => {

    const fetchEvents = async () => {
      try {
        const response = await fetch("http://localhost:5097/api/v1/events");
        const data = await response.json();
        setEvents(
          data.map((event: any) => ({
            title: event.title,
            eventId: event.eventId,
            start: new Date(event.eventDate + "T" + event.startTime), // Combine DateOnly and TimeSpan
            end: new Date(event.eventDate + "T" + event.endTime),     // Combine DateOnly and TimeSpan
          }))
        );
      } catch (err) {
        console.error("Error fetching events:", err);
      }
    };
  
    fetchEvents();
  }, []);

  // Handle navigation to the CreateEvent page
  const handleCreateEvent = () => {
    window.location.href = "/CreateEvent";
  };

  // Handle event click
  const handleEventClick = (event: any) => {
    // Navigate to the edit screen with the selected event's ID   
    navigate(`/edit-event/${event.eventId}`);
  };

  return (
    <div className="flex flex-col items-center h-screen bg-blue-100 p-4">
      <div className="flex justify-between items-center w-full max-w-4xl mb-4">
        <h1 className="text-2xl font-bold text-blue-700">Calendar</h1>
        <button
          onClick={handleCreateEvent}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition duration-200"
        >
          New
        </button>
      </div>

      <div className="bg-white shadow-lg rounded-lg p-4 w-full max-w-4xl">
        <Calendar
          localizer={localizer}
          events={events} // Calendar events
          startAccessor="start"
          endAccessor="end"
          style={{ height: 500 }}
          onSelectEvent={handleEventClick} // Handle event click
          defaultView="month"
        />
      </div>
    </div>
  );
};

export default AdminCalendarPage;