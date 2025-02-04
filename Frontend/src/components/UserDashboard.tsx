import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import UserDashboardSidebar from "./UserDashboardSidebar";
import CalendarPage from "./Calendar";
import { useUser } from "./UserContext";

const UserDashboard: React.FC = () => {
  const { userName, setUserId, setUserName, setRole } = useUser();
  const navigate = useNavigate();

  useEffect(() => {
    const handleBackButton = () => {
      console.log("User pressed back - Logging out");

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
    <div className="flex h-screen">
      {/* Sidebar */}
      <UserDashboardSidebar role="User" />

      {/* Main Content */}
      <div className="flex-1 bg-gray-100 p-8">
        {/* Welcome Text */}
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-800">
            Welcome, <span className="text-blue-600">{userName || "User"}</span>
            !
          </h1>
          <p className="text-gray-600 mt-2">
            Here's what's happening in your schedule:
          </p>
        </div>

        {/* Calendar Section */}
        <div className="bg-white rounded-lg shadow-lg p-6">
          <CalendarPage />
        </div>
      </div>
    </div>
  );
};

export default UserDashboard;
