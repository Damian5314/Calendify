import React, { useEffect, useState } from "react";
import EventFeedback from "./EventFeedback";

const EventDetails: React.FC<{ eventId: number }> = ({ eventId }) => {
  const [averageRating, setAverageRating] = useState<number | null>(null);

  useEffect(() => {
    const fetchAverageRating = async () => {
      try {
        const response = await fetch(`http://localhost:5097/api/v1/attendance/event/${eventId}/average-rating`);
        const data = await response.json();
        setAverageRating(data.averageRating);
      } catch (err) {
        console.error("Error fetching average rating:", err);
      }
    };

    fetchAverageRating();
  }, [eventId]);

  return (
    <div>
      <h1>Event Details</h1>
      <p>Average Rating: {averageRating !== null ? averageRating.toFixed(1) : "Loading..."}</p>
      {/* Include the EventFeedback component */}
      <EventFeedback eventId={eventId} userId={0} /> {/* Replace userId with dynamic value */}
    </div>
  );
};

export default EventDetails;
