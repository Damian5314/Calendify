import React, { useState } from "react";
import axios from "axios";

const ForgotPassword: React.FC = () => {
  const [email, setEmail] = useState(""); // State for email input
  const [message, setMessage] = useState(""); // Success message
  const [errorMessage, setErrorMessage] = useState(""); // Error message

  const handlePasswordReset = async () => {
    // Reset messages
    setMessage("");
    setErrorMessage("");

    // Validatie e-mail invoer
    const onlyLettersAndNumbersRegex = /^[a-zA-Z0-9]*$/; // Alleen letters en cijfers
    const onlyLettersRegex = /^[a-zA-Z]+$/; // Alleen letters

    if (!email) {
      // Als het veld leeg is
      setErrorMessage("Please enter your email.");
      return;
    } else if (onlyLettersAndNumbersRegex.test(email)) {
      // Als alleen letters en cijfers zonder '@' worden ingevoerd
      setErrorMessage("The email needs to include a special character like '@'.");
      return;
    } else if (onlyLettersRegex.test(email)) {
      // Als alleen letters worden ingevoerd
      setErrorMessage("Email must include numbers or special characters.");
      return;
    }

    // Probeer de reset-e-mail te verzenden
    try {
      const response = await axios.post(
        "http://localhost:5097/api/v1/auth/forgot-password",
        { email }
      );

      if (response.status === 200) {
        // Als de e-mail succesvol is verzonden
        setMessage("The email has been sent!");
      }
    } catch (error) {
      // Als er een algemene fout optreedt
      console.error("Error sending reset email:", error);
      setMessage("An Email has been send to you.");
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">
        Forgot Your Password?
      </h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <p className="text-gray-600 mb-4 text-center">
          Enter your email to reset your password.
        </p>

        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Enter your email"
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        {/* Success message with green color */}
        {message && (
          <p className="text-green-500 text-center mb-4">{message}</p>
        )}

        {/* Error message with specific colors */}
        {errorMessage && (
          <p
            className={`${
              errorMessage === "The email needs to include a special character like '@'." ||
              errorMessage === "Please enter your email."
                ? "text-red-500" // Rood voor specifieke fouten
                : "text-green-500" // Groen voor andere algemene fouten
            } text-center mb-4`}
          >
            {errorMessage}
          </p>
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
