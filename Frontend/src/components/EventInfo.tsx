import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import UserDashboardSidebar from "./UserDashboardSidebar";
import EventFeedback from "./EventFeedback";
import { useUser } from "./UserContext";

const EventInfo: React.FC = () => {
  const { userId } = useUser();
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
  const [isAttending, setIsAttending] = useState(false);
  const [showFeedbackForm, setShowFeedbackForm] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

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

    const fetchIsAttending = async () => {
      try {
        const respone = await fetch(
          `http://localhost:5097/api/v1/attendance/event/${eventId}/is-attending?userId=${userId}`
        );
        const attendanceData = await respone.json();
        setIsAttending(attendanceData.isAttending);
      } catch (error) {
        console.error("Error fetching event details:", error);
      }
    };

    fetchEvent();
    fetchAverageRating();
    fetchIsAttending();
  }, [eventId, userId]);

  const handleCancel = () => {
    navigate(-1);
  };

  const handleFeedbackSubmit = async (rating: number, feedback: string) => {
    try {
      const response = await fetch("http://localhost:5097/api/v1/attendance/feedback", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ eventId, userId , feedback, rating }),
      });

      if (response.ok) {
        setSuccessMessage("Feedback submitted successfully!");
        setErrorMessage("");
        setShowFeedbackForm(false);
        const updatedRating = await fetchAverageRating();
        setAverageRating(updatedRating);
      } else {
        setErrorMessage("Failed to submit feedback.");
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
  
  const handleAttendEvent = async () => {
    if (!userId) {
      alert("User not logged in! Please log in to attend this event.");
      return;
    }
  
    try {
      const response = await fetch("http://localhost:5097/api/v1/attendance", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ eventId, userId }),
      });
  
      if (response.ok) {
        setSuccessMessage("Event attended successfully!");
        setErrorMessage("");
        setIsAttending(true);
      } else {
        setErrorMessage("Failed to attend event.");
      }
    } catch (err) {
      console.error("Error attending event:", err);
    }
  };

  const handleLeaveEvent = async () => {
    if (!userId) {
      alert("User not logged in! Please log in to attend this event.");
      return;
    }
  
    try {
      const response = await fetch("http://localhost:5097/api/v1/attendance", {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ eventId, userId }),
      });
  
      if (response.ok) {
        setSuccessMessage("Left event successfully!");
        setErrorMessage("");
        setIsAttending(false);
      } else {
        setErrorMessage("Failed to leave event.");
      }
    } catch (err) {
      console.error("Error leaving event:", err);
    }
  };

  return (
    <div className="flex">
      <UserDashboardSidebar role="User" />

      <div className="flex-1 flex flex-col items-center justify-center bg-blue-100 p-6">
        <div className="bg-white shadow-lg rounded-lg w-full max-w-3xl p-6">
          <div className="flex justify-between items-center mb-6">
            <h2 className="text-3xl font-bold text-blue-700">Event Details</h2>
            {isAttending && (
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
            )}
          </div>
          <div className="space-y-6">
            <div>
              <label className="block text-gray-700 font-medium">Title</label>
              <input
                type="text"
                value={eventData.title}
                disabled
                className="border border-gray-300 p-2 rounded w-full bg-gray-100"
              />
            </div>
            <div>
              <label className="block text-gray-700 font-medium">Description</label>
              <textarea
                value={eventData.description}
                disabled
                className="border border-gray-300 p-2 rounded w-full bg-gray-100"
              />
            </div>
            <div>
              <label className="block text-gray-700 font-medium">Date</label>
              <input
                type="date"
                value={eventData.eventDate}
                disabled
                className="border border-gray-300 p-2 rounded w-full bg-gray-100"
              />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-gray-700 font-medium">Start Time</label>
                <input
                  type="time"
                  value={eventData.startTime}
                  disabled
                  className="border border-gray-300 p-2 rounded w-full bg-gray-100"
                />
              </div>
              <div>
                <label className="block text-gray-700 font-medium">End Time</label>
                <input
                  type="time"
                  value={eventData.endTime}
                  disabled
                  className="border border-gray-300 p-2 rounded w-full bg-gray-100"
                />
              </div>
            </div>
            <div>
              <label className="block text-gray-700 font-medium">Location</label>
              <input
                type="text"
                value={eventData.location}
                disabled
                className="border border-gray-300 p-2 rounded w-full bg-gray-100"
                />
            </div>
          </div>
          {errorMessage && <p className="text-red-500 text-center mb-4">{errorMessage}</p>}
          {successMessage && <p className="text-green-500 text-center mb-4">{successMessage}</p>}
          <div className="flex justify-between mt-6">
            <button
              type="button"
              onClick={handleCancel}
              className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400 transition"
            >
              Back
            </button>
            <button
              type="button"
              onClick={isAttending? handleLeaveEvent : handleAttendEvent}
              className={`px-4 py-2 rounded transition ${
                isAttending ? "bg-red-500 hover:bg-red-600" : "bg-green-500 hover:bg-green-600"
              } text-white`}
            >
              {isAttending ? "Leave Event" : "Attend Event"}
            </button>
          </div>
        </div>
      </div>

      {showFeedbackForm && (
        <EventFeedback
          eventId={Number(eventId)}
          userId={1}
          onSubmit={handleFeedbackSubmit}
          onClose={() => setShowFeedbackForm(false)}
        />
      )}
    </div>
  );
};

export default EventInfo;