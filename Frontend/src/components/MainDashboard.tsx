import React, { useEffect, useState } from "react";
import { getSessionRole } from "../api/api";
import { useNavigate } from "react-router-dom";

const MainDashboard = () => {
  const navigate = useNavigate();
  const [role, setRole] = useState<string | null>(null);

  useEffect(() => {
    const fetchRole = async () => {
      try {
        const response = await getSessionRole();
        setRole(response.data);
      } catch {
        navigate("/login");
      }
    };
    fetchRole();
  }, [navigate]);

  if (role === "Admin") navigate("/admin-dashboard");
  if (role === "User") navigate("/user-dashboard");

  return <div>Loading...</div>;
};

export default MainDashboard;
