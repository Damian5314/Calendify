import React from "react";
import UserDashboardSidebar from "./UserDashboardSidebar";
import CalendarPage from "./Calendar";

const UserDashboard: React.FC = () => {
  const userName = ""; // Replace with dynamic data from backend
  const role = "User"; // Replace with dynamic data from backend

  return (
    <div className="flex h-screen">
      {/* Sidebar */}
      <UserDashboardSidebar userName={userName} role={role} />

      {/* Main Content */}
      <div className="flex-1 bg-gray-100 p-8">
        {/* Welcome Text */}
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-800">
            Welcome<span className="text-blue-600">{userName}</span>!
          </h1>
          <p className="text-gray-600 mt-2">
            Here's what's happening in your schedule:
          </p>
        </div>

        {/* Calendar Section */}
        <div className="bg-white rounded-lg shadow-lg p-6">
          <CalendarPage />
        </div>
      </div>
    </div>
  );
};

export default UserDashboard;
