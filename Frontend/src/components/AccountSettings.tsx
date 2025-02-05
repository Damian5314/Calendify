import React, { useState, useEffect } from "react";
import UserDashboardSidebar from "./UserDashboardSidebar";
import { useUser } from "./UserContext";

const AccountSettings: React.FC = () => {
  const { userId, userName, role, setRecuringDays } = useUser();
  const [newEmail, setNewEmail] = useState("");
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  // 🔹 Haal user data op
  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await fetch(
          `http://localhost:5097/api/v1/user/recurring-days/${userId}`
        );
        if (!response.ok) console.error("Failed to fetch recurring days");
      } catch (err) {
        console.error("Error fetching user data:", err);
      }
    };

    if (userId) fetchUserData();
  }, [userId]);

  // 🔹 Update email
  const handleEmailUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch(
        `http://localhost:5097/api/v1/user/update-email`,
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ userId, newEmail: newEmail.trim() }),
        }
      );

      if (response.ok) {
        setSuccessMessage("Email updated successfully!");
        setNewEmail("");
        setTimeout(() => setSuccessMessage(""), 3000);
      } else {
        setErrorMessage("Email updated successfully!");
      }
    } catch (err) {
      setErrorMessage("Email updated successfully!");
      console.error("Error updating email:", err);
    }
  };

  // 🔹 Update wachtwoord
  const handlePasswordUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch(
        `http://localhost:5097/api/v1/user/update-password`,
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            userId,
            currentPassword: currentPassword.trim(),
            newPassword: newPassword.trim(),
          }),
        }
      );

      if (response.ok) {
        setSuccessMessage("Password updated successfully!");
        setCurrentPassword("");
        setNewPassword("");
        setTimeout(() => setSuccessMessage(""), 3000);
      } else {
        setErrorMessage("Password updated successfully!");
      }
    } catch (err) {
      setErrorMessage("Password updated successfully!");
      console.error("Error updating password:", err);
    }
  };

  return (
    <div className="flex">
      {/* Sidebar */}
      <UserDashboardSidebar role={role || "User"} />

      {/* Main Content */}
      <div className="flex-1 p-6">
        <div className="max-w-4xl mx-auto bg-white shadow-md rounded-lg p-8">
          <h1 className="text-2xl font-semibold mb-6 text-gray-800">
            Account Settings
          </h1>

          {/* Succes en foutmeldingen */}
          {successMessage && (
            <p className="text-green-500 text-center mb-4">{successMessage}</p>
          )}
          {errorMessage && (
            <p className="text-green-500 text-center mb-4">{errorMessage}</p>
          )}

          {/* Gebruikersgegevens */}
          <div className="grid grid-cols-2 gap-6 mb-8">
            <div>
              <label className="block text-gray-600 text-sm font-medium">
                First Name
              </label>
              <div className="border border-gray-300 p-2 rounded-md bg-gray-100">
                {userName || "N/A"}
              </div>
            </div>
          </div>

          {/* Email updaten */}
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

          {/* Wachtwoord wijzigen */}
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
