import React from "react";
import { useNavigate } from "react-router-dom";

const MainDashboard: React.FC = () => {
  const navigate = useNavigate(); // To navigate between pages

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-4xl font-bold text-blue-700 mb-8">
        Welcome to Calendify
      </h1>
      <p className="text-lg text-gray-600 mb-6">
        Plan and organize your events with ease.
      </p>

      <div className="space-x-4">
        <button
          onClick={() => navigate("/login")}
          className="bg-blue-500 text-white px-6 py-3 rounded-lg font-semibold hover:bg-blue-600 transition duration-200"
        >
          Login
        </button>
        <button
          onClick={() => navigate("/register")}
          className="bg-green-500 text-white px-6 py-3 rounded-lg font-semibold hover:bg-green-600 transition duration-200"
        >
          Register
        </button>
      </div>
    </div>
  );
};

export default MainDashboard;
