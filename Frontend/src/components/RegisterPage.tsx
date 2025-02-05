import React, { useState } from "react";
import axios from "axios";

const RegisterPage: React.FC = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    role: "User", // Default role for registering users
    recuringDays: [] as string[], // ⚡ Veranderd naar een array!
  });

  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  // Dagen die een gebruiker kan kiezen
  const daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

  // Input handler voor tekstvelden
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  // Handler voor selecteren/deselecteren van dagen
  const handleDaySelection = (day: string) => {
    setFormData((prev) => ({
      ...prev,
      recuringDays: prev.recuringDays.includes(day)
        ? prev.recuringDays.filter((d) => d !== day) // Verwijderen als al geselecteerd
        : [...prev.recuringDays, day], // Toevoegen als niet geselecteerd
    }));
  };

  // Registratie-verzoek versturen
  const handleRegister = async () => {
    const { firstName, lastName, email, password, recuringDays } = formData;

    // Validatie
    if (!firstName || !lastName || !email || !password || recuringDays.length === 0) {
      setErrorMessage("All fields are required.");
      return;
    }

    try {
      await axios.post("http://localhost:5097/api/v1/auth/register", formData);
      setSuccessMessage("Registration successful! You can now log in.");
      setErrorMessage(""); // Vorige errors wissen
      setTimeout(() => (window.location.href = "/login"), 2000);
    } catch (error: any) {
      setErrorMessage(error.response?.data || "Registration failed. Please try again.");
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">Create an Account</h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <h2 className="text-2xl font-semibold text-center mb-4">Register</h2>

        <input
          type="text"
          name="firstName"
          placeholder="First Name"
          value={formData.firstName}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded"
        />

        <input
          type="text"
          name="lastName"
          placeholder="Last Name"
          value={formData.lastName}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded"
        />

        <input
          type="email"
          name="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded"
        />

        <input
          type="password"
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleInputChange}
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded"
        />

        {/* 🔹 Keuze voor meerdere terugkerende dagen */}
        <div className="mb-4">
          <label className="text-sm font-medium text-gray-700">Select Recurring Days:</label>
          <div className="flex flex-wrap gap-2 mt-2">
            {daysOfWeek.map((day) => (
              <button
                key={day}
                onClick={() => handleDaySelection(day)}
                className={`px-4 py-2 rounded ${
                  formData.recuringDays.includes(day) ? "bg-blue-500 text-white" : "bg-gray-200 text-blue-500"
                }`}
              >
                {day}
              </button>
            ))}
          </div>
        </div>

        {errorMessage && <p className="text-red-500 text-center mb-4">{errorMessage}</p>}
        {successMessage && <p className="text-green-500 text-center mb-4">{successMessage}</p>}

        <button onClick={handleRegister} className="bg-blue-500 text-white w-full py-2 rounded font-semibold hover:bg-blue-600">
          Register
        </button>
      </div>
    </div>
  );
};

export default RegisterPage;
