import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import AdminDashboardSidebar from "./AdminDashboardSidebar";
import EventFeedback from "./EventFeedback";

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
  const [averageRating, setAverageRating] = useState<number | null>(null);
  const [showFeedbackForm, setShowFeedbackForm] = useState(false);

  // Fetch the event details and average rating
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

    const fetchAverageRating = async () => {
      try {
        const response = await fetch(
          `http://localhost:5097/api/v1/attendance/event/${eventId}/average-rating`
        );
        const data = await response.json();
        setAverageRating(data.averageRating);
      } catch (err) {
        console.error("Error fetching average rating:", err);
      }
    };

    fetchEvent();
    fetchAverageRating();
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

  const handleFeedbackSubmit = async (rating: number, feedback: string) => {
    try {
      const response = await fetch("http://localhost:5097/api/v1/attendance/feedback", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ eventId, userId: 1, feedback, rating }), // Replace `userId: 1` with dynamic value
      });

      if (response.ok) {
        alert("Feedback submitted successfully!");
        setShowFeedbackForm(false);
        const updatedRating = await fetchAverageRating(); // Refresh average rating
        setAverageRating(updatedRating);
      } else {
        alert("Failed to submit feedback.");
      }
    } catch (err) {
      console.error("Error submitting feedback:", err);
    }
  };

  const fetchAverageRating = async (): Promise<number> => {
    try {
      const response = await fetch(
        `http://localhost:5097/api/v1/attendance/event/${eventId}/average-rating`
      );
      const data = await response.json();
      return data.averageRating;
    } catch (err) {
      console.error("Error fetching average rating:", err);
      return 0;
    }
  };

  return (
    <div className="flex">
      <AdminDashboardSidebar />

      <div className="flex-1 flex flex-col items-center justify-center bg-blue-100 p-6">
        <div className="bg-white shadow-lg rounded-lg w-full max-w-3xl p-6">
          <div className="flex justify-between items-center mb-6">
            <h2 className="text-3xl font-bold text-blue-700">Edit Event</h2>
            <div>
              <span className="text-gray-600">
                Average Rating: {averageRating !== null ? averageRating.toFixed(1) : "Loading..."}
              </span>
              <button
                onClick={() => setShowFeedbackForm(true)}
                className="ml-4 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
              >
                Rate Event
              </button>
            </div>
          </div>
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

      {showFeedbackForm && (
        <EventFeedback
          eventId={Number(eventId)}
          userId={1} // Replace with dynamic userId
          onSubmit={handleFeedbackSubmit}
          onClose={() => setShowFeedbackForm(false)}
        />
      )}
    </div>
  );
};

export default EditEvent;
