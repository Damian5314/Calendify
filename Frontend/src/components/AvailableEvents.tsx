import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom"; // Import useNavigate
import { FaCalendarAlt, FaClock, FaMapMarkerAlt } from "react-icons/fa";
import UserDashboardSidebar from "./UserDashboardSidebar";

interface Event {
  id: number;
  title: string;
  description: string;
  eventDate: string;
  startTime: string;
  endTime: string;
  location: string;
}

const AvailableEvents: React.FC = () => {
  const [events, setEvents] = useState<Event[]>([]);
  const [error, setError] = useState("");
  const navigate = useNavigate(); // Initialize useNavigate

  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const response = await fetch("http://localhost:5097/api/v1/events");
        if (!response.ok) {
          throw new Error("Failed to fetch events");
        }
        const data = await response.json();

        // Get today's date
        const today = new Date().toISOString().split("T")[0];

        // Filter to show only upcoming events
        const upcomingEvents = data.filter((event: Event) => event.eventDate >= today);

        setEvents(upcomingEvents);
      } catch (error) {
        setError("Failed to load events. Please try again later.");
        console.error(error);
      }
    };

    fetchEvents();
  }, []);

  return (
    <div className="flex">
      {/* Sidebar */}
      <UserDashboardSidebar role="User" />

      {/* Main Content */}
      <div className="ml-64 p-8 bg-gray-100 min-h-screen w-full">
        <h1 className="text-3xl font-bold text-gray-800 mb-6">Available Events</h1>

        {error && <p className="text-red-600">{error}</p>}

        <div className="bg-white shadow-lg rounded-lg p-6">
          {events.length === 0 ? (
            <p className="text-gray-600">No upcoming events available.</p>
          ) : (
            <ul className="space-y-4">
              {events.map((event) => (
                <li
                  key={event.id}
                  className="p-4 border border-gray-300 rounded-lg bg-gray-50 shadow-sm hover:shadow-md transition duration-300 cursor-pointer"
                  onClick={() => navigate(`/event-info/${event.eventId}`)} // Navigate to EventInfo page
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
      </div>
    </div>
  );
};

export default AvailableEvents;
