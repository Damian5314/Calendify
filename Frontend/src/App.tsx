import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import MainDashboard from "./components/MainDashboard";
import LoginPage from "./components/LoginPage";
import RegisterPage from "./components/RegisterPage";
import AdminDashboard from "./components/AdminDashboard";
import UserDashboard from "./components/UserDashboard";
import ForgotPassword from "./components/ForgotPassword";
import CreateEvent from "./components/CreateEvent";
import CalendarPage from "./components/Calendar";
import AdminCalendarPage from "./components/AdminCalendar";
import NewPassword from "./components/NewPassword";
import UserData from "./components/UserData";
import EditEvent from "./components/EditEvent";
import CreateAdmin from "./components/CreateAdmin";
import EventInfo from "./components/EventInfo";
import ProtectedRoute from "./components/ProtectedRoute"; // Import the ProtectedRoute

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        {/* Public routes */}
        <Route path="/" element={<MainDashboard />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/reset-password" element={<NewPassword />} />

        {/* Protected routes */}
        <Route
          path="/admin-dashboard"
          element={
            <ProtectedRoute>
              <AdminDashboard />
            </ProtectedRoute>
          }
        />
        <Route
          path="/user-dashboard"
          element={
            <ProtectedRoute>
              <UserDashboard />
            </ProtectedRoute>
          }
        />
        <Route
          path="/CreateEvent"
          element={
            <ProtectedRoute>
              <CreateEvent />
            </ProtectedRoute>
          }
        />
        <Route
          path="/Calendar"
          element={
            <ProtectedRoute>
              <CalendarPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/AdminCalendar"
          element={
            <ProtectedRoute>
              <AdminCalendarPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/UserData"
          element={
            <ProtectedRoute>
              <UserData />
            </ProtectedRoute>
          }
        />
        <Route
          path="/edit-event/:eventId"
          element={
            <ProtectedRoute>
              <EditEvent />
            </ProtectedRoute>
          }
        />
        <Route
          path="/CreateAdmin"
          element={
            <ProtectedRoute>
              <CreateAdmin />
            </ProtectedRoute>
          }
        />
        <Route
          path="/EventInfo/:eventId"
          element={
            <ProtectedRoute>
              <EventInfo />
            </ProtectedRoute>
          }
        />
      </Routes>
    </Router>
  );
};

export default App;
