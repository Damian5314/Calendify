import React, { useState } from "react";
import axios from "axios";

const RegisterPage: React.FC = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    role: "User", // Default role for registering users
    recuringDays: "",
  });
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleRegister = async () => {
    const { firstName, lastName, email, password, recuringDays } = formData;

    // Validate input fields
    if (!firstName || !lastName || !email || !password || !recuringDays) {
      setErrorMessage("All fields are required.");
      return;
    }

    try {
      const response = await axios.post(
        "http://localhost:5097/api/v1/auth/register",
        formData
      );
      setSuccessMessage("Registration successful! You can now log in.");
      setErrorMessage(""); // Clear any previous errors
      setTimeout(() => {
        window.location.href = "/login";
      }, 2000);
    } catch (error) {
      console.error("Registration error:", error);
      setErrorMessage(
        "Registration failed. Please check your input and try again."
      );
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">
        Create an Account
      </h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <h2 className="text-2xl font-semibold text-center mb-4">Register</h2>

        <input
          type="text"
          name="firstName"
          placeholder="First Name"
          value={formData.firstName}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        <input
          type="text"
          name="lastName"
          placeholder="Last Name"
          value={formData.lastName}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        <input
          type="email"
          name="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        <input
          type="password"
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        <input
          type="text"
          name="recuringDays"
          placeholder="Recuring Days (e.g., Monday, Wednesday)"
          value={formData.recuringDays}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        {errorMessage && (
          <p className="text-red-500 text-center mb-4">{errorMessage}</p>
        )}

        {successMessage && (
          <p className="text-green-500 text-center mb-4">{successMessage}</p>
        )}

        <button
          onClick={handleRegister}
          className="bg-blue-500 text-white w-full py-2 rounded font-semibold hover:bg-blue-600 transition duration-200"
        >
          Register
        </button>
      </div>
    </div>
  );
};

export default RegisterPage;
