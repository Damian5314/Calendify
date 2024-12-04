import React, { useState } from "react";

const Login: React.FC = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loginMessage, setLoginMessage] = useState("");

    const handleLogin = async () => {
        try {
            const response = await fetch("http://localhost:5097/api/v1/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                credentials: "include", // Ensure cookies are included
                body: JSON.stringify({ email, password }),
            });

            if (response.ok) {
                setLoginMessage("Login successful!");
            } else {
                const errorMessage = await response.text();
                setLoginMessage(`Error: ${errorMessage}`);
            }
        } catch (error) {
            console.error("Error during login:", error);
            setLoginMessage("An error occurred during login.");
        }
    };

    const checkLoginStatus = async () => {
        try {
            const response = await fetch("http://localhost:5097/api/v1/auth/is-logged-in", {
                method: "GET",
                credentials: "include", // Ensure cookies are sent
            });

            if (response.ok) {
                const message = await response.text();
                setLoginMessage(message); // Example: "Logged in as User" or "Logged in as Admin"
            } else {
                setLoginMessage("You are not logged in.");
            }
        } catch (error) {
            console.error("Error checking login status:", error);
            setLoginMessage("An error occurred while checking login status.");
        }
    };

    return (
        <div>
            <h1>Login</h1>
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button onClick={handleLogin}>Login</button>
            <button onClick={checkLoginStatus}>Check Login Status</button>
            <p>{loginMessage}</p>
        </div>
    );
};

export default Login;
