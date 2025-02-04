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
  const storedRole = localStorage.getItem("role");

  // Check if role exists either in state or localStorage
  if (!role && !storedRole) {
    return <Navigate to="/login" replace />;
  }

  // Use storedRole if role is null (ensures protection after refresh)
  const userRole = role || storedRole;

  if (!allowedRoles.includes(userRole!)) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
