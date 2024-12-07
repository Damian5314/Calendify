import React, { useState } from "react";

const UserDashboard = () => {
  const [successMessage, setSuccessMessage] = useState("");

  const handleUpdate = () => {
    setSuccessMessage("Your information has been updated!");
  };

  return (
    <div className="p-8 bg-blue-50 min-h-screen">
      <h1 className="text-2xl font-bold">User Dashboard</h1>
      <button
        onClick={handleUpdate}
        className="bg-green-500 text-white px-4 py-2 rounded"
      >
        Update Information
      </button>
      {successMessage && (
        <p className="text-green-600 mt-4">{successMessage}</p>
      )}
    </div>
  );
};

export default UserDashboard;
