import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./components/LoginPage";
import RegisterPage from "./components/RegisterPage";
import UserDashboard from "./components/UserDashboard";
import AdminDashboard from "./components/AdminDashboard";
import ForgotPassword from "./components/ForgotPassword";
import CreateEvent from "./components/CreateEvent";
import CalendarPage from "./components/Calendar";
import AdminCalendarPage from "./components/AdminCalendar";
import NewPassword from "./components/NewPassword";
import UserData from "./components/UserData";
import EditEvent from "./components/EditEvent";
import CreateAdmin from "./components/CreateAdmin";
import EventInfo from "./components/EventInfo";
import ProtectedRoute from "./components/ProtectedRoute";
import { UserProvider } from "./components/UserContext";

const App: React.FC = () => {
  return (
    <UserProvider>
      <Router>
        <Routes>
          {/* Public Routes */}
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/reset-password" element={<NewPassword />} />

          {/* Protected Routes */}
          <Route
            path="/user-dashboard"
            element={
              <ProtectedRoute allowedRoles={["User"]}>
                <UserDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin-dashboard"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <AdminDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/CreateEvent"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <CreateEvent />
              </ProtectedRoute>
            }
          />
          <Route
            path="/Calendar"
            element={
              <ProtectedRoute allowedRoles={["User"]}>
                <CalendarPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/AdminCalendar"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <AdminCalendarPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/UserData"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <UserData />
              </ProtectedRoute>
            }
          />
          <Route
            path="/edit-event/:eventId"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <EditEvent />
              </ProtectedRoute>
            }
          />
          <Route
            path="/CreateAdmin"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <CreateAdmin />
              </ProtectedRoute>
            }
          />
          <Route
            path="/EventInfo/:eventId"
            element={
              <ProtectedRoute allowedRoles={["User"]}>
                <EventInfo />
              </ProtectedRoute>
            }
          />
        </Routes>
      </Router>
    </UserProvider>
  );
};

export default App;
