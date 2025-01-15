import React, { useState } from "react";

const EventFeedback: React.FC<{ eventId: number; userId: number }> = ({ eventId, userId }) => {
  const [feedback, setFeedback] = useState("");
  const [rating, setRating] = useState(0);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = await fetch("http://localhost:5097/api/v1/attendance/feedback", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ eventId, userId, feedback, rating }),
      });

      if (response.ok) {
        alert("Feedback submitted successfully!");
      } else {
        alert("Error submitting feedback.");
      }
    } catch (err) {
      console.error("Error:", err);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Rating:</label>
        <select value={rating} onChange={(e) => setRating(Number(e.target.value))}>
          <option value={0}>Select a rating</option>
          {[1, 2, 3, 4, 5].map((value) => (
            <option key={value} value={value}>
              {value} Stars
            </option>
          ))}
        </select>
      </div>
      <div>
        <label>Feedback:</label>
        <textarea
          value={feedback}
          onChange={(e) => setFeedback(e.target.value)}
          placeholder="Write your feedback here..."
        ></textarea>
      </div>
      <button type="submit">Submit Feedback</button>
    </form>
  );
};

export default EventFeedback;
