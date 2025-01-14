import React, { useEffect, useState } from "react";
import UserDashboardSidebar from "./UserDashboardSidebar";
import CalendarPage from "./Calender";

const UserDashboard: React.FC = () => {
  const [userName, setUserName] = useState<string>("");
  const [role, setRole] = useState<string>("");

  useEffect(() => {
    const fetchUserInfo = async () => {
      try {
        const response = await fetch("http://localhost:5097/api/v1/user-info", {
          credentials: "include", // Include cookies for session tracking
        });
        if (response.ok) {
          const data = await response.json();
          setUserName(data.FirstName);
          setRole(data.Role);
        } else {
          console.error("Failed to fetch user info");
        }
      } catch (error) {
        console.error("Error fetching user info:", error);
      }
    };

    fetchUserInfo();
  }, []);

  return (
    <div className="flex h-screen">
      <UserDashboardSidebar userName={userName} role={role} />
      <div className="flex-1 bg-gray-100 p-8">
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-800">
            Welcome back, <span className="text-blue-600">{userName}</span>!
          </h1>
          <p className="text-gray-600 mt-2">
            Here's what's happening in your schedule:
          </p>
        </div>
        <div className="bg-white rounded-lg shadow-lg p-6">
          <CalendarPage />
        </div>
      </div>
    </div>
  );
};

export default UserDashboard;
