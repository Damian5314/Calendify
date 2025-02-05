import React, { useEffect, useState } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useUser } from "./UserContext";

interface ProtectedRouteProps {
  allowedRoles: string[];
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  allowedRoles,
  children,
}) => {
  const { role, userId, setUserId, setRole, setUserName } = useUser();
  const [isLoading, setIsLoading] = useState(true);
  const location = useLocation();

  useEffect(() => {
    // Check localStorage for user details
    const storedUserId = localStorage.getItem("userId");
    const storedRole = localStorage.getItem("role");
    const storedUserName = localStorage.getItem("userName");

    if (storedUserId && storedRole && storedUserName) {
      setUserId(parseInt(storedUserId, 10));
      setRole(storedRole);
      setUserName(storedUserName);
    }

    setIsLoading(false);
  }, [setUserId, setRole, setUserName]);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  // Allow direct access to reset-password page
  if (location.pathname === "/reset-password") {
    return <>{children}</>;
  }

  // Redirect to login if no valid session or role mismatch
  if (!role || !userId || !allowedRoles.includes(role)) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
