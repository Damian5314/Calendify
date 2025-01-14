import React, { useState, useEffect } from "react";

const UserData: React.FC = () => {
  const daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
  const [activeTab, setActiveTab] = useState("allUsers");
  const [users, setUsers] = useState<any[]>([]);

  // Fetch users based on the active tab
  useEffect(() => {
    const fetchUsers = async () => {
      try {
        // Build the API URL dynamically based on the active tab
        let url = "http://localhost:5097/api/v1/accounts";
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
  }, [activeTab]); // Re-fetch users whenever the active tab changes

  const renderTabContent = () => {
    return (
      <div>
        <h2 className="text-lg font-bold mb-2">
          {activeTab === "allUsers" ? "All Users" : `Users on ${activeTab}`}
        </h2>
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
    <div className="flex flex-col items-center h-screen bg-blue-100 p-4">
      <h1 className="text-2xl font-bold text-blue-700 mb-4">User Management</h1>

      {/* Tabs */}
      <div className="flex space-x-4 mb-4">
        <button
          onClick={() => setActiveTab("allUsers")}
          className={`px-4 py-2 rounded ${
            activeTab === "allUsers"
              ? "bg-blue-500 text-white"
              : "bg-gray-200 text-blue-500"
          }`}
        >
          All Users
        </button>
        {daysOfWeek.map((day) => (
          <button
            key={day}
            onClick={() => setActiveTab(day)}
            className={`px-4 py-2 rounded ${
              activeTab === day
                ? "bg-blue-500 text-white"
                : "bg-gray-200 text-blue-500"
            }`}
          >
            {day}
          </button>
        ))}
      </div>

      {/* Tab Content */}
      <div className="bg-white shadow-lg rounded-lg p-4 w-full max-w-4xl">
        {renderTabContent()}
      </div>
    </div>
  );
};

export default UserData;