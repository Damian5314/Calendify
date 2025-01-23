import React from "react";
import { FaUserCog, FaUsers, FaCalendarAlt, FaSignOutAlt } from "react-icons/fa";

const AdminDashboardSidebar: React.FC = () => {
  const handleLogout = async () => {
    try {
      const response = await fetch("http://localhost:5097/api/v1/auth/Logout", {
        method: "POST",
        credentials: "include",
      });

      if (response.ok) {
        window.location.href = "/login";
      } else {
        alert("Failed to log out. Please try again.");
      }
    } catch (error) {
      console.error("Logout error:", error);
      alert("An error occurred while logging out. Please try again.");
    }
  };

  return (
    <div className="bg-gradient-to-br from-blue-100 via-green-50 to-white shadow-lg rounded-lg p-6 w-64 h-full">
      <h2 className="text-2xl font-bold text-gray-800 mb-2">
        Welcome<span className="text-blue-600">{}</span>!
      </h2>
      <p className="text-sm text-gray-600 mb-6">
        Role: User<strong>{}</strong>
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
      </ul>

      <button
            onClick={handleLogout}
            className="mt-6 flex items-center space-x-2 text-red-600 hover:text-red-800 font-medium transition duration-200"
        >
            <FaSignOutAlt className="text-red-600" />
            <span>Logout</span>
        </button>
    </div>
  );
};

export default AdminDashboardSidebar;
