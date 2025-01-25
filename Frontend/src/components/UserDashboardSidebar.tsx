import React from "react";
import { FaCalendarAlt, FaSignOutAlt } from "react-icons/fa";
import { useNavigate } from "react-router-dom"; // Import useNavigate for redirection
import { useUser } from "./UserContext";

const UserDashboardSidebar: React.FC<{ role: string }> = ({
  role,
}) => {
  const { userName } = useUser();
  const navigate = useNavigate(); // Initialize navigate

  const handleLogout = async () => {
    try {
      const response = await fetch("http://localhost:5097/api/v1/auth/logout", {
        method: "POST",
        credentials: "include", // Include cookies for authentication
      });

      if (response.ok) {
        navigate("/login"); // Redirect to login screen
      } else {
        alert("Failed to log out. Please try again.");
      }
    } catch (error) {
      console.error("Error logging out:", error);
      alert("An error occurred. Please try again.");
    }
  };

  return (
    <div className="bg-gradient-to-br from-blue-100 via-green-50 to-white shadow-lg rounded-lg p-6 w-64 h-full">
      <h2 className="text-2xl font-bold text-gray-800 mb-2">
        Welcome<span className="text-blue-600"> {userName || "User"}</span>!
      </h2>
      <p className="text-sm text-gray-600 mb-6">
        Role: <strong>{role}</strong>
      </p>
      <hr className="mb-6 border-gray-300" />

      <ul className="space-y-4">
        <li className="flex items-center space-x-2">
          <FaCalendarAlt className="text-blue-600" />
          <a
            href="/user-dashboard"
            className="text-blue-800 hover:text-green-600 font-medium transition duration-200"
          >
            User Dashboard
          </a>
        </li>
        <li className="flex items-center space-x-2">
        </li>
        <li className="flex items-center space-x-2">
          <FaCalendarAlt className="text-blue-600" />
          <a
            href="/calendar"
            className="text-blue-800 hover:text-green-600 font-medium transition duration-200"
          >
            Event Calendar
          </a>
        </li>
        <li className="flex items-center space-x-2">
        </li>
      </ul>

      {/* Logout Button */}
      <button
        onClick={handleLogout}
        className="mt-6 flex items-center space-x-2 text-red-600 hover:text-red-800 font-medium transition duration-200"
      >
        <FaSignOutAlt className="text-red-600" />
        <span>Logout</span>
      </button>

      <div className="mt-6 p-4 bg-blue-50 border border-blue-200 rounded-lg">
        <p className="text-sm text-gray-600">
          🌿 Tip: Stay organized and plan your week effectively!
        </p>
      </div>
    </div>
  );
};

export default UserDashboardSidebar;
