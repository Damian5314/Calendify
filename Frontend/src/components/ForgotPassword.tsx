import React, { useState } from "react";
import axios from "axios";

const ForgotPassword: React.FC = () => {
  const [email, setEmail] = useState(""); // State for email input
  const [message, setMessage] = useState(""); // State for success/error messages
  const [errorMessage, setErrorMessage] = useState(""); // State for error messages

  const handlePasswordReset = async () => {
    if (!email) {
      setErrorMessage("Please enter your email.");
      return;
    }

    try {
      // Make API request to trigger password reset
      const response = await axios.post(
        "http://localhost:5097/api/v1/auth/forgot-password",
        { email }
      );

      // Display success message if request was successful
      setMessage(
        "If the email exists in our system, you will receive a password reset link."
      );
      setErrorMessage("");
    } catch (error) {
      console.error("Error sending password reset email:", error);
      setErrorMessage("An error occurred. Please try again.");
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">
        Forgot Your Password?
      </h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <p className="text-gray-600 mb-4 text-center">
          Enter your email address, and we'll send you a link to reset your
          password.
        </p>

        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Enter your email"
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

export default ForgotPassword;
