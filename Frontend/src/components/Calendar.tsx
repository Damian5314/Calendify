import React, { useState, useEffect } from "react";
import { Calendar, momentLocalizer } from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { useNavigate } from "react-router-dom";

moment.updateLocale("en", {
  week: {
    dow: 1,
  },
});

const CalendarPage: React.FC = () => {
  const localizer = momentLocalizer(moment);
  const [events, setEvents] = useState<any[]>([]);
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
            start: new Date(event.eventDate + "T" + event.startTime),
            end: new Date(event.eventDate + "T" + event.endTime),
          }))
        );
      } catch (err) {
        console.error("Error fetching events:", err);
      }
    };

    fetchEvents();
  }, []);

  const handleEventClick = (event: any) => {
    navigate(`/event-info/${event.eventId}`);
  };

  return (
    <div className="flex flex-col items-center h-screen bg-blue-100 p-4">
      <div className="flex justify-between items-center w-full max-w-xl mb-4">
        <h1 className="text-2xl font-bold text-blue-700">Calendar</h1>
      </div>

      <div className="bg-white shadow-lg rounded-lg p-4 w-full max-w-4xl">
        <Calendar
          localizer={localizer}
          events={events}
          startAccessor="start"
          endAccessor="end"
          style={{ height: 600 }}
          onSelectEvent={handleEventClick}
          defaultView="month"
        />
      </div>
    </div>
  );
};

export default CalendarPage;
