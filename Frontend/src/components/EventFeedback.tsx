import React, { useState } from "react";

interface EventFeedbackProps {
  eventId: number;
  userId: number;
  onSubmit: (rating: number, feedback: string) => Promise<void>;
  onClose: () => void;
}

const EventFeedback: React.FC<EventFeedbackProps> = ({ eventId, userId, onSubmit, onClose }) => {
  const [feedback, setFeedback] = useState("");
  const [rating, setRating] = useState(0);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (rating === 0) {
      alert("Please select a rating.");
      return;
    }
    await onSubmit(rating, feedback);
  };

  return (
    <div className="fixed inset-0 bg-gray-800 bg-opacity-50 flex items-center justify-center">
      <div className="bg-white p-6 rounded-lg shadow-lg w-96">
        <h2 className="text-lg font-bold mb-4">Rate Event</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-gray-700 font-medium">Rating:</label>
            <select
              value={rating}
              onChange={(e) => setRating(Number(e.target.value))}
              className="border border-gray-300 p-2 rounded w-full"
            >
              <option value={0}>Select a rating</option>
              {[1, 2, 3, 4, 5].map((value) => (
                <option key={value} value={value}>
                  {value} Stars
                </option>
              ))}
            </select>
          </div>
          <div>
            <label className="block text-gray-700 font-medium">Feedback:</label>
            <textarea
              value={feedback}
              onChange={(e) => setFeedback(e.target.value)}
              placeholder="Write your feedback here..."
              className="border border-gray-300 p-2 rounded w-full"
            ></textarea>
          </div>
          <div className="flex justify-between">
            <button
              type="submit"
              className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
            >
              Submit
            </button>
            <button
              type="button"
              onClick={onClose}
              className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400 transition"
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EventFeedback;
