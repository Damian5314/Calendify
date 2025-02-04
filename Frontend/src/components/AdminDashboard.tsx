import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import AdminDashboardSidebar from "./AdminDashboardSidebar";
import AdminCalendarPage from "./AdminCalendar";
import { useUser } from "./UserContext";

const AdminDashboard: React.FC = () => {
  const { userName, setUserId, setUserName, setRole } = useUser();
  const navigate = useNavigate();

  useEffect(() => {
    const handleBackButton = () => {
      console.log("Admin pressed back - Logging out");

      // ✅ Clear session and logout
      fetch("http://localhost:5097/api/v1/auth/logout", {
        method: "POST",
        credentials: "include",
      })
        .then(() => {
          // ✅ Clear local storage & user context
          localStorage.removeItem("userId");
          localStorage.removeItem("userName");
          localStorage.removeItem("role");

          setUserId(null);
          setUserName(null);
          setRole(null);

          // ✅ Redirect to login page
          navigate("/login", { replace: true });
        })
        .catch((err) => console.error("Error during logout:", err));
    };

    window.addEventListener("popstate", handleBackButton); // 🔹 Detect back button

    return () => {
      window.removeEventListener("popstate", handleBackButton);
    };
  }, [navigate, setUserId, setUserName, setRole]);

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar */}
      <AdminDashboardSidebar />

      {/* Main Content */}
      <div className="flex-1 p-8">
        <h1 className="text-3xl font-bold text-gray-800 mb-6">
          Welcome to the Admin Dashboard
        </h1>
        <p className="text-lg text-gray-600 mb-4">
          Here you can manage users, events, and other administrative tasks.
        </p>

        {/* Calendar */}
        <div className="bg-white rounded-lg shadow-lg p-6">
          <AdminCalendarPage />
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
