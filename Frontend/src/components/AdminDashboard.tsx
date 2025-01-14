import React from "react";

const AdminDashboard: React.FC = () => {
  const handleUserData = () => {
    window.location.href = "/UserData";
  };

  const handleLogout = async () => {
    try {
      // Make a logout request to the backend
      const response = await fetch("http://localhost:5097/api/v1/auth/Logout", {
        method: "POST",
        credentials: "include", // Include cookies for session logout
      });

      if (response.ok) {
        // Redirect to the login page
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
    <div className="h-screen bg-blue-100 flex flex-col items-center justify-center">
      <h1 className="text-4xl font-bold text-blue-700 mb-6">Admin Dashboard</h1>
      <p className="text-lg text-gray-700 mb-4">
        Welcome to the Admin Dashboard! Here you can manage users and admins.
      </p>

      <div className="flex gap-4">
        <button
          className="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600 transition"
        >
          Create Admin
        </button>
        <button
          onClick={handleUserData}
          className="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600 transition"
        >
          View Users
        </button>
        <button
          onClick={handleLogout}
          className="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600 transition"
        >
          Logout
        </button>
      </div>
    </div>
  );
};

export default AdminDashboard;
