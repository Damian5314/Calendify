import React from "react";
import { useLocation, useNavigate } from "react-router-dom";

const Dashboard: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const role = location.state?.role;

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-50">
      <h1 className="text-3xl font-bold text-blue-600 mb-4">
        Welcome {role === "Admin" ? "Administrator" : "User"}
      </h1>
      <button
        onClick={() => navigate("/login")}
        className="mt-4 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
      >
        Logout
      </button>
    </div>
  );
};

export default Dashboard;
