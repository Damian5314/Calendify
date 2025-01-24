import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { useUser } from "./UserContext";

const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const { setUserId, setUserName } = useUser();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const handleLogin = async () => {
    if (!email || !password) {
      setErrorMessage("Please fill in both email and password.");
      return;
    }

    try {
      const response = await axios.post(
        "http://localhost:5097/api/v1/auth/login",
        {
          email,
          password,
        }
      );

      const { role, userId, firstName } = response.data;

      setUserId(userId);
      setUserName(firstName);

      if (role === "Admin") {
        navigate("/admin-dashboard");
      } else if (role === "User") {
        navigate("/user-dashboard");
      } else {
        setErrorMessage("Unexpected role received from server.");
      }
    } catch (error) {
      console.error("Login error:", error);
      setErrorMessage("Invalid email or password. Please try again.");
    }
  };

  const handleRegisterRedirect = () => {
    navigate("/register"); // Redirect to the Register page
  };

  const handleForgotPasswordRedirect = () => {
    navigate("/forgot-password"); // Redirect to the Forgot Password page
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">
        Welcome to Calendify
      </h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <h2 className="text-2xl font-semibold text-center mb-4">Login</h2>

        {/* Email Input */}
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Enter your email"
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        {/* Password Input */}
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Enter your password"
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        {/* Forgot Password Button */}
        <div className="text-left mb-4">
          <button
            onClick={handleForgotPasswordRedirect}
            className="text-blue-500 text-sm hover:underline"
          >
            Forgot Password?
          </button>
        </div>

        {/* Error Message */}
        {errorMessage && (
          <p className="text-red-500 text-center mb-4">{errorMessage}</p>
        )}

        {/* Login Button */}
        <button
          onClick={handleLogin}
          className="bg-blue-500 text-white w-full py-2 rounded font-semibold hover:bg-blue-600 transition duration-200"
        >
          Login
        </button>

        {/* Register Button */}
        <button
          onClick={handleRegisterRedirect}
          className="mt-4 bg-gray-200 text-blue-500 w-full py-2 rounded font-semibold hover:bg-gray-300 transition duration-200"
        >
          Register
        </button>
      </div>
    </div>
  );
};

export default LoginPage;
