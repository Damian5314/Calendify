import React from "react";
import { Navigate } from "react-router-dom";
import { useUser } from "./UserContext";

interface ProtectedRouteProps {
  allowedRoles: string[];
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  allowedRoles,
  children,
}) => {
  const { role } = useUser();

  // Get the role from UserContext or localStorage
  const storedRole = role || localStorage.getItem("role");

  // If the user is not authorized, redirect to login
  if (!storedRole || !allowedRoles.includes(storedRole)) {
    return <Navigate to="/login" />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
