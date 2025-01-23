import React from "react";
import AdminDashboardSidebar from "./AdminDashboardSidebar"; // Import the sidebar
import AdminCalendarPage from "./AdminCalendar"; // Import the calendar component

const AdminDashboard: React.FC = () => {{
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

        {/* Calendar */}
        <div className="bg-white rounded-lg shadow-lg p-6">
          <h2 className="text-2xl font-semibold text-gray-800 mb-4">Calendar</h2>
          <AdminCalendarPage /> {/* Render the calendar here */}
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
