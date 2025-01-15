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
import AccountSettings from "./components/AccountSettings";
import NewPassword from "./components/NewPassword";
import UserData from "./components/UserData";
import EditEvent from "./components/EditEvent";
import CreateAdmin from "./components/CreateAdmin";
import EventInfo from "./components/EventInfo";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<MainDashboard />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/reset-password" element={<NewPassword />} /> {/* New route */}
        <Route path="/admin-dashboard" element={<AdminDashboard />} />
        <Route path="/user-dashboard" element={<UserDashboard />} />
        <Route path="/CreateEvent" element={<CreateEvent />} />
        <Route path="/account-settings" element={<AccountSettings />} />
        <Route path="/Calendar" element={<CalendarPage />} />
        <Route path="/AdminCalendar" element={<AdminCalendarPage />} />
        <Route path="/UserData" element={<UserData />} />
        <Route path="/edit-event/:eventId" element={<EditEvent />} />
        <Route path="/CreateAdmin" element={<CreateAdmin />} />
        <Route path="/EventInfo/:eventId" element={<EventInfo />} />
      </Routes>
    </Router>
  );
};

export default App;
