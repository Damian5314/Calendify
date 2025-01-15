import React, { useState } from "react";
import { useNavigate } from "react-router-dom";


const CreateEvent: React.FC = () => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [eventDate, setEventDate] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [location, setLocation] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formattedStartTime = `${startTime}:00`; // Append ":00" for seconds
    const formattedEndTime = `${endTime}:00`;

    const eventData = {
      title,
      description,
      eventDate,
      startTime: formattedStartTime,
      endTime: formattedEndTime,
      location,
      adminApproval: true,
      event_Attendances: [], // Optional, depending on backend requirements
    };
    try {
      const response = await fetch("http://localhost:5097/api/v1/events", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(eventData),
      });
      if (response.ok) {
        setSuccessMessage("Event created succesfully.");
        setErrorMessage(""); // Clear any previous errors
        setTimeout(() => {
          window.location.href = "/user-dashboard";
        }, 2000);
      } else {
        const error = await response.text();
        alert(`Error: ${error}`);
      }
    } catch (err) {
      console.error("Error creating event:", err);
      setErrorMessage("An error occurred while creating the event.");
    }
  };

  const handleCancel = () => {
    navigate(-1); 
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">Create Event</h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label htmlFor="title" className="block text-sm font-medium text-gray-700">
              Title *
            </label>
            <input
              type="text"
              id="title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
              className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
            />
          </div>
          <div className="mb-4">
            <label htmlFor="description" className="block text-sm font-medium text-gray-700">
              Description *
            </label>
            <textarea
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              required
              className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
            />
          </div>
          <div className="mb-4">
            <label htmlFor="eventDate" className="block text-sm font-medium text-gray-700">
              Date *
            </label>
            <input
              type="date"
              id="eventDate"
              value={eventDate}
              onChange={(e) => setEventDate(e.target.value)}
              required
              className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
            />
          </div>
          <div className="mb-4">
            <label htmlFor="startTime" className="block text-sm font-medium text-gray-700">
              Start Time *
            </label>
            <input
              type="time"
              id="startTime"
              value={startTime}
              onChange={(e) => setStartTime(e.target.value)}
              required
              className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
            />
          </div>
          <div className="mb-4">
            <label htmlFor="endTime" className="block text-sm font-medium text-gray-700">
              End Time *
            </label>
            <input
              type="time"
              id="endTime"
              value={endTime}
              onChange={(e) => setEndTime(e.target.value)}
              required
              className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
            />
          </div>
          <div className="mb-4">
            <label htmlFor="location" className="block text-sm font-medium text-gray-700">
              Location *
            </label>
            <input
              type="text"
              id="location"
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              required
              className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
            />
          </div>

          {errorMessage && (
          <p className="text-red-500 text-center mb-4">{errorMessage}</p>
          )}

          {successMessage && (
            <p className="text-green-500 text-center mb-4">{successMessage}</p>
          )}

          <button
            type="submit"
            className="bg-blue-500 text-white w-full py-2 rounded font-semibold hover:bg-blue-600 transition duration-200"
          >
            Create Event
          </button>
          <button
            type="button"
            onClick={handleCancel}
            className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400 w-full mt-2"
          >
            Cancel
          </button>
        </form>
      </div>
    </div>
  );
};

export default CreateEvent;