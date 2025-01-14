import React, { useState } from "react";
import { useSearchParams } from "react-router-dom";
import axios from "axios";

const NewPassword: React.FC = () => {
  const [password, setPassword] = useState(""); // New password input
  const [confirmPassword, setConfirmPassword] = useState(""); // Confirm password input
  const [message, setMessage] = useState(""); // Success message
  const [errorMessage, setErrorMessage] = useState(""); // Error message
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token"); // Extract token from query params

  const handlePasswordReset = async () => {
    if (!password || !confirmPassword) {
      setErrorMessage("Please fill in all fields.");
      return;
    }

    if (password !== confirmPassword) {
      setErrorMessage("Passwords do not match.");
      return;
    }

    try {
      const response = await axios.post(
        "http://localhost:5097/api/v1/auth/reset-password",
        { token, newPassword: password }
      );

      if (response.status === 200) {
        setMessage("Password reset successfully! You can now log in.");
        setErrorMessage("");
      }
    } catch (error) {
      console.error("Error resetting password:", error);
      setErrorMessage("An error occurred. Please try again.");
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">Reset Password</h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <p className="text-gray-600 mb-4 text-center">
          Enter your new password below.
        </p>

        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="New password"
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        <input
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          placeholder="Confirm new password"
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        {message && <p className="text-green-500 text-center mb-4">{message}</p>}
        {errorMessage && (
          <p className="text-red-500 text-center mb-4">{errorMessage}</p>
        )}

        <button
          onClick={handlePasswordReset}
          className="bg-blue-500 text-white w-full py-2 rounded font-semibold hover:bg-blue-600 transition duration-200"
        >
          Reset Password
        </button>

        <button
          onClick={() => (window.location.href = "/login")}
          className="mt-4 bg-gray-200 text-blue-500 w-full py-2 rounded font-semibold hover:bg-gray-300 transition duration-200"
        >
          Back to Login
        </button>
      </div>
    </div>
  );
};

export default NewPassword;
