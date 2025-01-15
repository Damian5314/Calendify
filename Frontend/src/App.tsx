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
import AccountSettings from "./components/AccountSettings";
import UserData from "./components/UserData";
import EditEvent from "./components/EditEvent";
import CreateAdmin from "./components/CreateAdmin";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/account-settings" element={<AccountSettings />} />
        <Route path="/" element={<MainDashboard />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/admin-dashboard" element={<AdminDashboard />} />
        <Route path="/user-dashboard" element={<UserDashboard />} />
        <Route path="/CreateEvent" element={<CreateEvent />} />
        <Route path="/Calendar" element={<CalendarPage />} />
        <Route path="/UserData" element={<UserData />} />
        <Route path="/edit-event/:eventId" element={<EditEvent />} />
        <Route path="/CreateAdmin" element={<CreateAdmin />} />
      </Routes>
    </Router>
  );
};

export default App;
