import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom"; 
import { FaCalendarAlt, FaClock, FaMapMarkerAlt } from "react-icons/fa";
import UserDashboardSidebar from "./UserDashboardSidebar";
import { useUser } from "./UserContext";
import moment from "moment";

interface Event_Attendance {
  userId: number;
  eventId: number;
}

interface Event {
  eventId: number;
  id: number;
  title: string;
  description: string;
  eventDate: string;
  startTime: string;
  endTime: string;
  location: string;
  event_Attendances: Event_Attendance[];
}

const AvailableEvents: React.FC = () => {
  const [events, setEvents] = useState<Event[]>([]);
  const [error, setError] = useState("");
  const { userId, recuringDays } = useUser();
  const navigate = useNavigate(); 

  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const response = await fetch("http://localhost:5097/api/v1/events");
        if (!response.ok) {
          throw new Error("Failed to fetch events");
        }
        const data = await response.json();

        const today = moment().format("YYYY-MM-DD");

        // 🔹 Alleen toekomstige events tonen
        const upcomingEvents = data.filter((event: Event) => event.eventDate >= today);

        setEvents(upcomingEvents);
      } catch (error) {  
        setError("Failed to load events. Please try again later.");
        console.error(error);
      }
    };

    fetchEvents();
  }, [userId, recuringDays]);

  // 🔹 Debugging log om te controleren welke dagen worden vergeleken
  console.log("User's recurring days:", recuringDays);

  // 🔹 Zorg ervoor dat beide dagen (gebruiker en event) lowercase en getrimd zijn
  const normalizeDay = (day: string) => day.trim().toLowerCase();

  // 🔹 Filter de events die de gebruiker kan bijwonen
  const eventsUserCanAttend = events.filter((event) => {
    const eventDay = normalizeDay(moment(event.eventDate, "YYYY-MM-DD").format("dddd"));
    return recuringDays.map(normalizeDay).includes(eventDay);
  });

  const handleEventClick = (event: Event) => {
    const eventDay = normalizeDay(moment(event.eventDate, "YYYY-MM-DD").format("dddd"));
    if (recuringDays.map(normalizeDay).includes(eventDay)) {
      navigate(`/event-info/${event.eventId}`);
    } else {
      alert("You can't attend this event. It is not on your recurring days.");
    }
  };

  return (
    <div className="flex">
      {/* Sidebar */}
      <UserDashboardSidebar role="User" />

      {/* Main Content */}
      <div className="flex-1 flex flex-col ml-64 p-8 bg-gray-100 min-h-screen w-full">
        <h1 className="text-3xl font-bold text-gray-800 mb-6">Available Events</h1>

        {error && <p className="text-red-600">{error}</p>}

        <div className="bg-white shadow-lg rounded-lg p-6">
          {/* 🔹 Alle komende evenementen */}
          <h2 className="text-xl font-bold text-gray-700 mb-4">All Upcoming Events</h2>
          {events.length === 0 ? (
            <p className="text-gray-600">No upcoming events available.</p>
          ) : (
            <ul className="space-y-4">
              {events.map((event) => (
                <li
                  key={event.eventId}
                  className="p-4 border border-gray-300 rounded-lg bg-gray-50 shadow-sm hover:shadow-md transition duration-300 cursor-pointer"
                  onClick={() => handleEventClick(event)}
                >
                  <h2 className="text-xl font-bold text-blue-600 flex items-center">
                    <FaCalendarAlt className="mr-2" /> {event.title}
                  </h2>
                  <p className="text-gray-700">{event.description}</p>
                  <p className="text-gray-500 flex items-center mt-2">
                    <FaClock className="mr-2" />
                    {event.eventDate} | {event.startTime} - {event.endTime}
                  </p>
                  <p className="text-gray-500 flex items-center">
                    <FaMapMarkerAlt className="mr-2" /> {event.location}
                  </p>
                </li>
              ))}
            </ul>
          )}
        </div>

        {/* 🔹 Events die de gebruiker KAN bijwonen */}
        <div className="bg-white shadow-lg rounded-lg p-6 mt-8 border-t-4 border-green-500">
          <h2 className="text-xl font-bold text-green-700 mb-4">Events You Can Attend</h2>
          {eventsUserCanAttend.length === 0 ? (
            <p className="text-gray-600">No events available based on your recurring days.</p>
          ) : (
            <ul className="space-y-4">
              {eventsUserCanAttend.map((event) => (
                <li
                  key={event.eventId}
                  className="p-4 border border-gray-300 rounded-lg bg-gray-50 shadow-sm hover:shadow-md transition duration-300 cursor-pointer"
                  onClick={() => navigate(`/event-info/${event.eventId}`)}
                >
                  <h2 className="text-xl font-bold text-green-600 flex items-center">
                    <FaCalendarAlt className="mr-2" /> {event.title}
                  </h2>
                  <p className="text-gray-700">{event.description}</p>
                  <p className="text-gray-500 flex items-center mt-2">
                    <FaClock className="mr-2" />
                    {event.eventDate} | {event.startTime} - {event.endTime}
                  </p>
                  <p className="text-gray-500 flex items-center">
                    <FaMapMarkerAlt className="mr-2" /> {event.location}
                  </p>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
};

export default AvailableEvents;
