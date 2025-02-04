import React from "react";
import { useUser } from "./UserContext";
import UserDashboardSidebar from "./UserDashboardSidebar";
import CalendarPage from "./Calendar";

const UserDashboard: React.FC = () => {
  const { userName } = useUser();

  return (
    <div className="flex h-screen">
      {/* Sidebar */}
      <UserDashboardSidebar role="User" />

      {/* Main Content */}
      <div className="flex-1 bg-gray-100 p-8">
        {/* Welcome Text */}
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-800">
            Welcome, <span className="text-blue-600">{userName || "User"}</span>
            !
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
