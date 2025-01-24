// ProtectedRoute.tsx
import React from "react";
import { Navigate } from "react-router-dom";
import { useUser } from "./UserContext";

const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const { userId } = useUser(); // Check if the user is logged in (userId is set)

  // If user is not logged in, redirect to the login page
  if (!userId) {
    return <Navigate to="/login" replace />;
  }

  // If logged in, render the children (protected content)
  return <>{children}</>;
};

export default ProtectedRoute;
