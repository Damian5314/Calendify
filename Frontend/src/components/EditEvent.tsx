import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import AdminDashboardSidebar from "./AdminDashboardSidebar";

const EditEvent: React.FC = () => {
  const { eventId } = useParams();
  const navigate = useNavigate();
  const [eventData, setEventData] = useState({
    title: "",
    description: "",
    eventDate: "",
    startTime: "",
    endTime: "",
    location: "",
  });
  const [attendees, setAttendees] = useState<{ 
    userId: number; 
    userName?: string;
    firstName?: string; 
    lastName?: string;
  }[]>([]);
  const [loading, setLoading] = useState(true);

  // Fetch event details and attendees
  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const response = await fetch(`http://localhost:5097/api/v1/events/${eventId}`);
        const data = await response.json();

        console.log("Fetched event data:", data); // Debugging

        setEventData(data); // Ensure all fields (adminApproval, event_Attendances) are included
        setAttendees(data.event_Attendances || []); // ✅ This now executes

      } catch (error) {
        console.error("Error fetching event:", error);
      } finally {
        setLoading(false);
      }
    };

  fetchEvent();
}, [eventId]);

useEffect(() => {
  const fetchUserNames = async () => {
    const updatedAttendees = await Promise.all(
      attendees.map(async (attendee) => {
        try {
          const response = await fetch(`http://localhost:5097/api/v1/users/${attendee.userId}`);
          const userData = await response.json();
          
          return { 
            ...attendee, 
            firstName: userData.firstName, 
            lastName: userData.lastName 
          };
        } catch (error) {
          console.error("Error fetching user:", error);
          return { 
            ...attendee, 
            firstName: "Unknown", 
            lastName: "User" 
          }; // Fallback name
        }
      })
    );

    setAttendees(updatedAttendees);
  };

  if (attendees.length > 0) {
    fetchUserNames();
  }
  }, [attendees]);

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
    if (!confirmation) return;

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
    navigate(-1);
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div className="flex">
      <AdminDashboardSidebar />

      {/* Main Content */}
      <div className="flex-1 flex flex-row items-start justify-center bg-blue-100 p-6">
        
        {/* Left Section - Event Form */}
        <div className="bg-white shadow-lg rounded-lg w-full max-w-2xl p-6">
          <h2 className="text-3xl font-bold text-blue-700 mb-4">Edit Event</h2>

          <form className="space-y-4" onSubmit={handleSubmit}>
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

            {/* Buttons */}
            <div className="flex justify-between mt-4">
              <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
                Save Changes
              </button>
              <button type="button" onClick={handleDelete} className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600">
                Delete Event
              </button>
              <button type="button" onClick={handleCancel} className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400">
                Cancel
              </button>
            </div>
          </form>
        </div>

        {/* Right Section - Attendee List */}
        <div className="ml-6 bg-white shadow-lg rounded-lg p-6 w-64">
          <h2 className="text-2xl font-bold text-blue-700">Attendees</h2>

          {attendees.length > 0 ? (
            <ul className="mt-4 space-y-2">
              {attendees.map((attendee) => (
                <li key={attendee.userId} className="p-2 bg-gray-100 rounded-lg">
                  {`${attendee.firstName} ${attendee.lastName} (ID: ${attendee.userId})`}
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-gray-600 mt-2">No users are attending this event.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default EditEvent;
