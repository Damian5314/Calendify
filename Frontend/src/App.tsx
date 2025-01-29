import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import MainDashboard from "./components/MainDashboard";
import LoginPage from "./components/LoginPage";
import RegisterPage from "./components/RegisterPage";
import ForgotPassword from "./components/ForgotPassword";
import NewPassword from "./components/NewPassword";
import AdminDashboard from "./components/AdminDashboard";
import UserDashboard from "./components/UserDashboard";
import CreateEvent from "./components/CreateEvent";
import CalendarPage from "./components/Calendar";
import AdminCalendarPage from "./components/AdminCalendar";
import UserData from "./components/UserData";
import EditEvent from "./components/EditEvent";
import CreateAdmin from "./components/CreateAdmin";
import EventInfo from "./components/EventInfo";
import ProtectedRoute from "./components/ProtectedRoute";
import AvailableEvents from "./components/AvailableEvents";
import { UserProvider } from "./components/UserContext";

const App: React.FC = () => {
  return (
    <UserProvider>
      <Router>
        <Routes>
          {/* Public Routes */}
          <Route path="/" element={<MainDashboard />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/reset-password" element={<NewPassword />} />

          {/* Protected Routes */}
          <Route
            path="/admin-dashboard"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <AdminDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/user-dashboard"
            element={
              <ProtectedRoute allowedRoles={["User"]}>
                <UserDashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/create-event"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <CreateEvent />
              </ProtectedRoute>
            }
          />
          <Route
            path="/calendar"
            element={
              <ProtectedRoute allowedRoles={["User"]}>
                <CalendarPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/admin-calendar"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <AdminCalendarPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/user-data"
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
            path="/create-admin"
            element={
              <ProtectedRoute allowedRoles={["Admin"]}>
                <CreateAdmin />
              </ProtectedRoute>
            }
          />
          <Route
            path="/event-info/:eventId"
            element={
              <ProtectedRoute allowedRoles={["User"]}>
                <EventInfo />
              </ProtectedRoute>
            }
          />
          <Route
            path="/available-events"
            element={
              <ProtectedRoute allowedRoles={["User"]}>
                <AvailableEvents />
              </ProtectedRoute>
            }
          />

          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Router>
    </UserProvider>
  );
};

export default App;
