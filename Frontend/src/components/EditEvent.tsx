import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import AdminDashboardSidebar from "./AdminDashboardSidebar"; // Or UserDashboardSidebar if universal

const EditEvent: React.FC = () => {
  const { eventId } = useParams(); // Get event ID from the URL
  const navigate = useNavigate();
  const [eventData, setEventData] = useState({
    title: "",
    description: "",
    eventDate: "",
    startTime: "",
    endTime: "",
    location: "",
  });

  // Fetch the event details
  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const response = await fetch(`http://localhost:5097/api/v1/events/${eventId}`);
        const data = await response.json();
        setEventData(data);
      } catch (error) {
        console.error("Error fetching event:", error);
      }
    };

    fetchEvent();
  }, [eventId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch(`http://localhost:5097/api/v1/events/${eventId}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(eventData),
      });

      if (response.ok) {
        alert("Event updated successfully!");
        navigate(-1);
      } else {
        alert("Failed to update event.");
      }
    } catch (error) {
      console.error("Error updating event:", error);
    }
  };

  const handleDelete = async () => {
    const confirmation = window.confirm("Are you sure you want to delete this event?");
  
    if (!confirmation) {
      return; // Exit if the user cancels the deletion
    }
    try {
      const response = await fetch(`http://localhost:5097/api/v1/events/${eventId}`, {
        method: "DELETE",
      });

      if (response.ok) {
        alert("Event deleted successfully!");
        navigate(-1);
      } else {
        alert("Failed to delete event.");
      }
    } catch (error) {
      console.error("Error deleting event:", error);
    }
  };

  const handleCancel = () => {
    navigate(-1); // Go back to the previous page
  };

  return (
    <div className="flex">
      {/* Include Admin Sidebar */}
      <AdminDashboardSidebar />

      {/* Main Content */}
      <div className="flex-1 flex flex-col items-center justify-center bg-blue-100 p-6">
        <div className="bg-white shadow-lg rounded-lg w-full max-w-3xl p-6">
          <h2 className="text-3xl font-bold mb-6 text-blue-700">Edit Event</h2>
          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label className="block text-gray-700 font-medium">Title</label>
              <input
                type="text"
                value={eventData.title}
                onChange={(e) => setEventData({ ...eventData, title: e.target.value })}
                className="border border-gray-300 p-2 rounded w-full"
              />
            </div>
            <div>
              <label className="block text-gray-700 font-medium">Description</label>
              <textarea
                value={eventData.description}
                onChange={(e) => setEventData({ ...eventData, description: e.target.value })}
                className="border border-gray-300 p-2 rounded w-full"
              />
            </div>
            <div>
              <label className="block text-gray-700 font-medium">Date</label>
              <input
                type="date"
                value={eventData.eventDate}
                onChange={(e) => setEventData({ ...eventData, eventDate: e.target.value })}
                className="border border-gray-300 p-2 rounded w-full"
              />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-gray-700 font-medium">Start Time</label>
                <input
                  type="time"
                  value={eventData.startTime}
                  onChange={(e) => setEventData({ ...eventData, startTime: e.target.value })}
                  className="border border-gray-300 p-2 rounded w-full"
                />
              </div>
              <div>
                <label className="block text-gray-700 font-medium">End Time</label>
                <input
                  type="time"
                  value={eventData.endTime}
                  onChange={(e) => setEventData({ ...eventData, endTime: e.target.value })}
                  className="border border-gray-300 p-2 rounded w-full"
                />
              </div>
            </div>
            <div>
              <label className="block text-gray-700 font-medium">Location</label>
              <input
                type="text"
                value={eventData.location}
                onChange={(e) => setEventData({ ...eventData, location: e.target.value })}
                className="border border-gray-300 p-2 rounded w-full"
              />
            </div>
            <div className="flex justify-between">
              <button
                type="submit"
                className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
              >
                Save Changes
              </button>
              <button
                type="button"
                onClick={handleDelete}
                className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition"
              >
                Delete Event
              </button>
              <button
                type="button"
                onClick={handleCancel}
                className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400 transition"
              >
                Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default EditEvent;
