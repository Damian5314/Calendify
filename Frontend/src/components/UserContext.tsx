import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";

interface UserContextType {
  userId: string | null;
  userName: string | null;
  role: string | null; // Add role to the context
  setUserId: (id: string | null) => void;
  setUserName: (name: string | null) => void;
  setRole: (role: string | null) => void; // Add setter for role
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [userId, setUserIdState] = useState<string | null>(null);
  const [userName, setUserNameState] = useState<string | null>(null);
  const [role, setRoleState] = useState<string | null>(null);

  useEffect(() => {
    const storedUserId = localStorage.getItem("userId");
    const storedUserName = localStorage.getItem("userName");
    const storedRole = localStorage.getItem("role"); // Load role from localStorage
    if (storedUserId) setUserIdState(storedUserId);
    if (storedUserName) setUserNameState(storedUserName);
    if (storedRole) setRoleState(storedRole);
  }, []);

  const setUserId = (id: string | null) => {
    if (id) localStorage.setItem("userId", id);
    else localStorage.removeItem("userId");
    setUserIdState(id);
  };

  const setUserName = (name: string | null) => {
    if (name) localStorage.setItem("userName", name);
    else localStorage.removeItem("userName");
    setUserNameState(name);
  };

  const setRole = (role: string | null) => {
    if (role) localStorage.setItem("role", role);
    else localStorage.removeItem("role");
    setRoleState(role);
  };

  return (
    <UserContext.Provider
      value={{ userId, userName, role, setUserId, setUserName, setRole }}
    >
      {children}
    </UserContext.Provider>
  );
};

export const useUser = (): UserContextType => {
  const context = useContext(UserContext);
  if (!context) throw new Error("useUser must be used within a UserProvider");
  return context;
};
