import React, { useEffect, useState } from "react";
import EventFeedback from "./EventFeedback";

const EventDetails: React.FC<{ eventId: number }> = ({ eventId }) => {
  const [averageRating, setAverageRating] = useState<number | null>(null);
  const [showFeedbackForm, setShowFeedbackForm] = useState(false);

  useEffect(() => {
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

    fetchAverageRating();
  }, [eventId]);

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
        // Refresh average rating
        const updatedRating = await fetchAverageRating();
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
    <div>
      <h1>Event Details</h1>
      <p>Average Rating: {averageRating !== null ? averageRating.toFixed(1) : "Loading..."}</p>
      <button
        onClick={() => setShowFeedbackForm(true)}
        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
      >
        Leave Feedback
      </button>
      {showFeedbackForm && (
        <EventFeedback
          eventId={eventId}
          userId={1} // Replace with dynamic userId
          onSubmit={handleFeedbackSubmit}
          onClose={() => setShowFeedbackForm(false)}
        />
      )}
    </div>
  );
};

export default EventDetails;
