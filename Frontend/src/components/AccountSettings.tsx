import React, { useState, useEffect } from "react";
import UserDashboardSidebar from "./UserDashboardSidebar"; // Import the sidebar component

const AccountSettings: React.FC = () => {
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: "",
    recurringDays: "",
  });
  const [newEmail, setNewEmail] = useState("");
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");

  // Fetch user data on page load
  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await fetch("http://localhost:5097/api/v1/user/profile", {
          method: "GET",
          credentials: "include",
        });
        if (response.ok) {
          const data = await response.json();
          console.log("User Data:", data);
          setUser(data);
        } else {
          console.error("Failed to fetch user data");
        }
      } catch (err) {
        console.error("Error:", err);
      }
    };

    fetchUserData();
  }, []);

  // Handle email update
  const handleEmailUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch(
        "http://localhost:5097/api/v1/users/update-email",
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
          body: JSON.stringify({ email: newEmail }),
        }
      );

      if (response.ok) {
        alert("Email updated successfully!");
        setUser((prev) => ({ ...prev, email: newEmail }));
        setNewEmail("");
      } else {
        alert("Failed to update email.");
      }
    } catch (err) {
      console.error("Error updating email:", err);
    }
  };

  // Handle password update
  const handlePasswordUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch(
        "http://localhost:5097/api/v1/users/update-password",
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
          body: JSON.stringify({ currentPassword, newPassword }),
        }
      );

      if (response.ok) {
        alert("Password updated successfully!");
        setCurrentPassword("");
        setNewPassword("");
      } else {
        alert("Failed to update password.");
      }
    } catch (err) {
      console.error("Error updating password:", err);
    }
  };

  return (
    <div className="flex">
      {/* Sidebar */}
      <UserDashboardSidebar userName={user.firstName} role="User" />

      {/* Main Content */}
      <div className="flex-1 p-6">
        <div className="max-w-4xl mx-auto bg-white shadow-md rounded-lg p-8">
          <h1 className="text-2xl font-semibold mb-6 text-gray-800">
            Account Settings
          </h1>

          {/* Display User Information */}
          <div className="grid grid-cols-2 gap-6 mb-8">
            <div>
              <label className="block text-gray-600 text-sm font-medium">
                First Name
              </label>
              <div className="border border-gray-300 p-2 rounded-md bg-gray-100">
                {user.firstName || "N/A"}
              </div>
            </div>
            <div>
              <label className="block text-gray-600 text-sm font-medium">
                Last Name
              </label>
              <div className="border border-gray-300 p-2 rounded-md bg-gray-100">
                {user.lastName || "N/A"}
              </div>
            </div>
            <div>
              <label className="block text-gray-600 text-sm font-medium">
                Email
              </label>
              <div className="border border-gray-300 p-2 rounded-md bg-gray-100">
                {user.email || "N/A"}
              </div>
            </div>
            <div>
              <label className="block text-gray-600 text-sm font-medium">
                Recurring Days
              </label>
              <div className="border border-gray-300 p-2 rounded-md bg-gray-100">
                {user.recurringDays || "N/A"}
              </div>
            </div>
          </div>

          {/* Update Email Form */}
          <form onSubmit={handleEmailUpdate} className="mb-8">
            <h2 className="text-lg font-semibold mb-4">Update Email</h2>
            <div className="flex items-center gap-4">
              <input
                type="email"
                value={newEmail}
                onChange={(e) => setNewEmail(e.target.value)}
                placeholder="Enter new email"
                className="border border-gray-300 rounded-md p-2 w-full"
                required
              />
              <button
                type="submit"
                className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 transition"
              >
                Update
              </button>
            </div>
          </form>

          {/* Update Password Form */}
          <form onSubmit={handlePasswordUpdate}>
            <h2 className="text-lg font-semibold mb-4">Change Password</h2>
            <div className="grid grid-cols-2 gap-4">
              <input
                type="password"
                value={currentPassword}
                onChange={(e) => setCurrentPassword(e.target.value)}
                placeholder="Current password"
                className="border border-gray-300 rounded-md p-2 w-full"
                required
              />
              <input
                type="password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                placeholder="New password"
                className="border border-gray-300 rounded-md p-2 w-full"
                required
              />
            </div>
            <button
              type="submit"
              className="mt-4 bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 transition"
            >
              Change Password
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default AccountSettings;
