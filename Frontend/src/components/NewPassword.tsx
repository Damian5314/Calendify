import React, { useState } from "react";
import axios from "axios";

const NewPassword: React.FC = () => {
  const [email, setEmail] = useState(""); // Email for identification
  const [password, setPassword] = useState(""); // New password input
  const [confirmPassword, setConfirmPassword] = useState(""); // Confirmation input
  const [message, setMessage] = useState(""); // Success message
  const [errorMessage, setErrorMessage] = useState(""); // Error message

  const validatePassword = (password: string): boolean => {
    const passwordRegex = /^(?=.*[A-Z])(?=.*\d).{8,}$/; // Minimum rules
    return passwordRegex.test(password);
  };

  const handlePasswordChange = async () => {
    if (!validatePassword(password)) {
      setErrorMessage("Password must contain at least 8 characters, one uppercase letter, and one number.");
      setMessage("");
      return;
    }

    if (password !== confirmPassword) {
      setErrorMessage("Passwords do not match.");
      setMessage("");
      return;
    }

    try {
      const response = await axios.post("http://localhost:5097/api/v1/auth/reset-password", {
        email,
        password,
      });

      if (response.status === 200) {
        setMessage("Password successfully changed!");
        setErrorMessage("");
      } else {
        setErrorMessage("An error occurred while changing your password. Please try again.");
      }
    } catch (error) {
      console.error("Error resetting password:", error);
      setErrorMessage("Failed to reset password. Please try again.");
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">Reset Your Password</h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <p className="text-gray-600 mb-4 text-center">
          Please enter your email and new password. Make sure it meets the requirements below.
        </p>

        <div className="mb-4">
          <label htmlFor="email" className="block text-sm font-medium text-gray-700">
            Email
          </label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
          />
        </div>

        <div className="mb-4">
          <label htmlFor="password" className="block text-sm font-medium text-gray-700">
            New Password
          </label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
          />
        </div>

        <div className="mb-4">
          <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700">
            Confirm Password
          </label>
          <input
            type="password"
            id="confirmPassword"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
          />
        </div>

        <p className="text-sm text-gray-500 mb-4">
          Password must contain at least 8 characters, one uppercase letter, and one number.
        </p>

        {errorMessage && <p className="text-red-500 text-center mb-4">{errorMessage}</p>}
        {message && <p className="text-green-500 text-center mb-4">{message}</p>}

        <button
          onClick={handlePasswordChange}
          className="bg-blue-500 text-white w-full py-2 rounded font-semibold hover:bg-blue-600 transition duration-200"
        >
          Change Password
        </button>

        {message && (
          <button
            onClick={() => (window.location.href = "/login")}
            className="mt-4 bg-gray-200 text-blue-500 w-full py-2 rounded font-semibold hover:bg-gray-300 transition duration-200"
          >
            Back to Login
          </button>
        )}
      </div>
    </div>
  );
};

export default NewPassword;
