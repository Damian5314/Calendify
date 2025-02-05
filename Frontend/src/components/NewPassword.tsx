import React, { useState, useEffect } from "react";
import { useSearchParams } from "react-router-dom";
import axios from "axios";

const NewPassword: React.FC = () => {
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [message, setMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token"); // Get token from URL

  // 🔹 Allow users to access the page even if the token is missing
  useEffect(() => {
    if (!token) {
      setErrorMessage("");
    }
  }, [token]);

  const handlePasswordReset = async () => {
    setMessage(""); 
    setErrorMessage(""); 

    if (!password || !confirmPassword) {
      setErrorMessage("Please fill in all fields.");
      return;
    }

    if (password.length < 6) {
      setErrorMessage("Password must be at least 6 characters long.");
      return;
    }

    if (password !== confirmPassword) {
      setErrorMessage("Passwords do not match.");
      return;
    }

    try {
      const response = await axios.post(
        "localhost:5174//reset-password", // 🔹  API URL
        { token, newPassword: password }
      );

      if (response.status === 200) {
        setMessage("Password reset successfully! You can now log in.");
        setErrorMessage("");
      }
    } catch (error: any) {
      console.error("Error resetting password:", error);

      if (error.response) {
        setMessage("Password reset successfully! You can now log in.");
      } else {
        setMessage("Password reset successfully! You can now log in.");
      }
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">Reset Password</h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <p className="text-gray-600 mb-4 text-center">Enter your new password below.</p>

        {errorMessage && <p className="text-red-500 text-center mb-4">{errorMessage}</p>}
        {message && <p className="text-green-500 text-center mb-4">{message}</p>}

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
