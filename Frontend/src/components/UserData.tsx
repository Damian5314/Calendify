import React, { useState, useEffect } from "react";
import AdminDashboardSidebar from "./AdminDashboardSidebar"; // Import the admin sidebar

const UserData: React.FC = () => {
  const [activeTab, setActiveTab] = useState("allUsers");
  const [users, setUsers] = useState<any[]>([]);

  // Fetch users based on the active tab
  useEffect(() => {
    const fetchUsers = async () => {
      try {
        let url = "http://localhost:5097/api/v1/admin/users";
        if (activeTab !== "allUsers") {
          url += `?day=${activeTab}`;
        }

        const response = await fetch(url);
        const data = await response.json();
        setUsers(data);
      } catch (err) {
        console.error("Error fetching users:", err);
      }
    };

    fetchUsers();
  }, [activeTab]);

  const renderTabContent = () => {
    return (
      <div>
        <h2 className="text-lg font-bold mb-2 capitalize">{activeTab}</h2>
        <ul>
          {users.map((user, index) => (
            <li key={index} className="border-b py-2">
              {user.FirstName} {user.LastName} ({user.Email})
            </li>
          ))}
        </ul>
      </div>
    );
  };

  return (
    <div className="flex h-screen">
      {/* Sidebar */}
      <AdminDashboardSidebar />

      {/* Main Content */}
      <div className="flex-1 p-8 bg-blue-100">
        <h1 className="text-3xl font-bold text-gray-800 mb-6">User Management</h1>

        {/* Tabs */}
        <div className="flex space-x-2 mb-4">
          {["allUsers", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"].map((day) => (
            <button
              key={day}
              onClick={() => setActiveTab(day)}
              className={`px-4 py-2 rounded ${
                activeTab === day
                  ? "bg-blue-500 text-white"
                  : "bg-gray-200 text-blue-500"
              }`}
            >
              {day === "allUsers" ? "All Users" : day}
            </button>
          ))}
        </div>

        {/* Tab Content */}
        <div className="bg-white shadow-lg rounded-lg p-4">
          {renderTabContent()}
        </div>
      </div>
    </div>
  );
};

export default UserData;
