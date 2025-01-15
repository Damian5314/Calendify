import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const CreateAdmin: React.FC = () => {
    const [adminData, setAdminData] = useState({
        userName: "",
        email: "",
        password: "",
    });

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await fetch("http://localhost:5097/api/v1/auth/register-admin", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(adminData),
            });
    
            if (response.ok) {
                alert("Admin registered successfully!");
                navigate("/admin-dashboard");
            } else {
                const error = await response.json();
                alert(`Failed to register admin: ${error.message}`);
            }
        } catch (err) {
            console.error("Error registering admin:", err);
        }
    };

    const handleCancel = () => {
        navigate(-1); 
    };
    return (
        <div className="flex flex-col items-center justify-center h-screen bg-gray-100">
            <form
                onSubmit={handleSubmit}
                className="bg-white p-6 rounded-lg shadow-lg w-96"
            >
                <h2 className="text-2xl font-bold mb-4">Create Admin</h2>
                <input
                    type="text"
                    placeholder="Username"
                    value={adminData.userName}
                    onChange={(e) =>
                        setAdminData({ ...adminData, userName: e.target.value })
                    }
                    className="w-full mb-4 p-2 border rounded"
                    required
                />
                <input
                    type="email"
                    placeholder="Email"
                    value={adminData.email}
                    onChange={(e) =>
                        setAdminData({ ...adminData, email: e.target.value })
                    }
                    className="w-full mb-4 p-2 border rounded"
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={adminData.password}
                    onChange={(e) =>
                        setAdminData({ ...adminData, password: e.target.value })
                    }
                    className="w-full mb-4 p-2 border rounded"
                    required
                />
                <button
                    type="submit"
                    className="bg-blue-500 text-white px-4 py-2 rounded w-full"
                >
                    Register Admin
                </button>
                <button
                type="button"
                onClick={handleCancel}
                className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400 w-full mt-2"
            >
                Cancel
                </button>
            </form>
        </div>
    );
};

export default CreateAdmin;
