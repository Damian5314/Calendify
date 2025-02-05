import React, { useState, useEffect } from "react"; // Importeer React en zijn hooks
import axios from "axios"; // Importeer axios voor API-aanroepen
import { useNavigate } from "react-router-dom"; // Importeer useNavigate hook voor navigatie
import { useUser } from "./UserContext"; // Importeer useUser hook voor gebruikersgegevens

const LoginPage: React.FC = () => {
  const { setUserId, setUserName, setRole } = useUser(); // Gebruik useUser hook om gebruikersgegevens te krijgen
  const [email, setEmail] = useState(""); // Stel email state in op leeg
  const [password, setPassword] = useState(""); // Stel password state in op leeg
  const [errorMessage, setErrorMessage] = useState(""); // Stel error message state in op leeg
  const [rememberMe, setRememberMe] = useState(false); // Stel remember me state in op false

  const navigate = useNavigate(); // Gebruik useNavigate hook voor navigatie

  useEffect(() => {
    // Controleer of er een actieve sessie is met behulp van de refresh token
    axios
      .post(
        "http://localhost:5097/api/v1/auth/refresh-token",
        {},
        { withCredentials: true }
      )
      .then((res) => {
        setUserId(res.data.UserId); // Stel user ID in
        setUserName(res.data.FirstName); // Stel user naam in
        setRole(res.data.Role); // Stel user rol in
        if (res.data.Role === "Admin") {
          navigate("/admin-dashboard"); // Navigeer naar admin dashboard
        } else if (res.data.Role === "User") {
          navigate("/user-dashboard"); // Navigeer naar user dashboard
        }
      })
      .catch(() => console.log("Geen actieve sessie.")); // Log geen actieve sessie
  }, []);

  const handleLogin = async () => {
    // Controleer of email en password zijn ingevuld
    if (!email || !password) {
      setErrorMessage("Vul beide email en password in."); // Stel error message in
      return;
    }

    try {
      const response = await axios.post(
        "http://localhost:5097/api/v1/auth/login",
        {
          email,
          password,
          rememberMe,
        },
        { withCredentials: true }
      );

      const { role, userId, firstName } = response.data;

      setUserId(userId); // Stel user ID in
      setUserName(firstName); // Stel user naam in
      setRole(role); // Stel user rol in

      if (role === "Admin") {
        navigate("/admin-dashboard"); // Navigeer naar admin dashboard
      } else if (role === "User") {
        navigate("/user-dashboard"); // Navigeer naar user dashboard
      } else {
        setErrorMessage("Onverwachte rol ontvangen van server."); // Stel error message in
        return;
      }
    } catch (error) {
      console.error("Login fout:", error); // Log login fout
      setErrorMessage("Ongeldig email of password. Probeer opnieuw."); 
    }
  };

  const handleRegisterRedirect = () => {
    navigate("/register"); // Navigeer naar registratie pagina
  };

  const handleForgotPasswordRedirect = () => {
    navigate("/forgot-password"); // Navigeer naar wachtwoord vergeten pagina
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-blue-100">
      <h1 className="text-3xl font-bold mb-6 text-blue-700">
        Welcome to Calendify
      </h1>

      <div className="bg-white shadow-lg rounded-lg p-8 w-96">
        <h2 className="text-2xl font-semibold text-center mb-4">Login</h2>

        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Enter your email"
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Enter your password"
          className="mb-4 w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-300"
        />

        {/* Adding 'Remember Me' checkbox */}
        <label className="flex items-center mb-4">
          <input
            type="checkbox"
            checked={rememberMe}
            onChange={() => setRememberMe(!rememberMe)}
            className="mr-2"
          />
          Remember Me
        </label>

        {errorMessage && (
          <p className="text-red-500 text-center mb-4">{errorMessage}</p>
        )}

        <button
          onClick={handleLogin}
          className="bg-blue-500 text-white w-full py-2 rounded font-semibold hover:bg-blue-600 transition duration-200"
        >
          Login
        </button>

        <div className="flex justify-between mt-4">
          <button
            onClick={handleForgotPasswordRedirect}
            className="text-blue-500 hover:underline"
          >
            Forgot Password?
          </button>
          <button
            onClick={handleRegisterRedirect}
            className="text-blue-500 hover:underline"
          >
            Register
          </button>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
