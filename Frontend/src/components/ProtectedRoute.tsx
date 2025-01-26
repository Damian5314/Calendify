import React, { useEffect, useState } from "react";
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
  const { role, userId, setUserId, setRole, setUserName } = useUser();
  const [isLoading, setIsLoading] = useState(true);

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

    setIsLoading(false); // Ensure ProtectedRoute only evaluates after checking localStorage
  }, [setUserId, setRole, setUserName]);

  // While checking session details, show a loading state
  if (isLoading) {
    return <div>Loading...</div>;
  }

  // Redirect to login if no valid session or role mismatch
  if (!role || !userId || !allowedRoles.includes(role)) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
