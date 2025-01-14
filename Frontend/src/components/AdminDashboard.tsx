import React from "react";
import AdminDashboardSidebar from "./AdminDashboardSidebar"; // Import the sidebar
import CalendarPage from "./Calendar"; // Import the calendar component

const AdminDashboard: React.FC = () => {
  const handleCreateAdmin = () => {
    alert("Redirect to create admin functionality");
  };

  const handleViewUsers = () => {
    window.location.href = "/user-management";
  };

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar */}
      <AdminDashboardSidebar />

      {/* Main Content */}
      <div className="flex-1 p-8">
        <h1 className="text-3xl font-bold text-gray-800 mb-6">Welcome to the Admin Dashboard</h1>
        <p className="text-lg text-gray-600 mb-4">
          Here you can manage users, events, and other administrative tasks.
        </p>

        {/* Buttons */}
        <div className="grid grid-cols-2 gap-4 mb-8">
          <button
            onClick={handleCreateAdmin}
            className="bg-blue-500 text-white px-6 py-3 rounded-md shadow-md hover:bg-blue-600 transition"
          >
            Create Admin
          </button>
          <button
            onClick={handleViewUsers}
            className="bg-blue-500 text-white px-6 py-3 rounded-md shadow-md hover:bg-blue-600 transition"
          >
            View Users
          </button>
        </div>

        {/* Calendar */}
        <div className="bg-white rounded-lg shadow-lg p-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">Calendar</h2>
          <CalendarPage /> {/* Render the calendar here */}
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
