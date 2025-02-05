import React, { useEffect, useState } from "react"; // Importeer React en zijn hooks
import { Navigate } from "react-router-dom"; // Importeer Navigate component van react-router-dom
import { useUser } from "./UserContext"; // Importeer useUser hook uit UserContext

interface ProtectedRouteProps {
  // Definieer interface voor ProtectedRoute component
  allowedRoles: string[]; // Array van toegestane rollen
  children: React.ReactNode; // Kinderen van de component
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  // Definieer ProtectedRoute component
  allowedRoles, // Toegestane rollen
  children, // Kinderen van de component
}) => {
  const { role, userId, setUserId, setRole, setUserName } = useUser(); // Gebruik useUser hook om user data te krijgen
  const [isLoading, setIsLoading] = useState(true); // Stel isLoading state in op true

  useEffect(() => {
    // Gebruik useEffect hook om localStorage te controleren
    // Controleer localStorage voor gebruikersgegevens
    const storedUserId = localStorage.getItem("userId"); // Haal userId op uit localStorage
    const storedRole = localStorage.getItem("role"); // Haal role op uit localStorage
    const storedUserName = localStorage.getItem("userName"); // Haal userName op uit localStorage

    if (storedUserId && storedRole && storedUserName) {
      // Als alle gegevens aanwezig zijn
      setUserId(parseInt(storedUserId, 10)); // Stel userId in
      setRole(storedRole); // Stel role in
      setUserName(storedUserName); // Stel userName in
    }

    setIsLoading(false); // Stel isLoading state in op false
  }, [setUserId, setRole, setUserName]); // Doe dit alleen als setUserId, setRole of setUserName verandert

  // Toon een laadstatus terwijl de sessiegegevens worden gecontroleerd
  if (isLoading) {
    return <div>Laden...</div>; // Toon laadstatus
  }

  // Redirect naar login als er geen geldige sessie is of de rol niet overeenkomt
  if (!role || !userId || !allowedRoles.includes(role)) {
    return <Navigate to="/login" replace />; // Redirect naar login pagina
  }

  return <>{children}</>; // Toon kinderen van de component
};

export default ProtectedRoute; // Exporteer ProtectedRoute component
