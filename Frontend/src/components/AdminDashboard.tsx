import React from "react";

const AdminDashboard: React.FC = () => {
  return (
    <div className="h-screen bg-blue-100 flex flex-col items-center justify-center">
      <h1 className="text-4xl font-bold text-blue-700 mb-6">Admin Dashboard</h1>
      <p className="text-lg text-gray-700 mb-4">
        Welcome to the Admin Dashboard! Here you can manage users and admins.
      </p>

      <div className="flex gap-4">
        <button className="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600 transition">
          Create Admin
        </button>
        <button className="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600 transition">
          View Users
        </button>
        <button className="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600 transition">
          Logout
        </button>
      </div>
    </div>
  );
};

export default AdminDashboard;
