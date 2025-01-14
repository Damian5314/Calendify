import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getSessionRole } from "../api/api";

const MainDashboard = () => {
  const navigate = useNavigate();
  const [role, setRole] = useState<string | null>(null);

  useEffect(() => {
    const fetchRole = async () => {
      try {
        const response = await getSessionRole();
        setRole(response.data);
      } catch (error) {
        // Redirect to introduction page if not logged in
        navigate("/");
      }
    };
    fetchRole();
  }, [navigate]);

  // Redirect based on user role
  useEffect(() => {
    if (role === "Admin") navigate("/admin-dashboard");
    if (role === "User") navigate("/user-dashboard");
  }, [role, navigate]);

  // Render the introduction page while determining user role
  if (role === null) {
    return (
      <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
        <h1 className="text-4xl font-bold text-blue-700 mb-6">
          Welcome to Calendify
        </h1>
        <p className="text-lg text-gray-600 mb-8 text-center">
          Your time, organized! Manage your schedule effortlessly.
        </p>

        <div className="space-x-4">
          <button
            onClick={() => navigate("/login")}
            className="bg-blue-500 text-white py-2 px-6 rounded font-semibold hover:bg-blue-600 transition duration-200"
          >
            Login
          </button>
          <button
            onClick={() => navigate("/register")}
            className="bg-gray-200 text-blue-500 py-2 px-6 rounded font-semibold hover:bg-gray-300 transition duration-200"
          >
            Register
          </button>
        </div>
      </div>
    );
  }
  // Fallback loading UI
  return <div>Loading...</div>;
};

export default MainDashboard;
