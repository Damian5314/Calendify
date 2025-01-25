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
  role: string | null;
  setUserId: (id: string | null) => void;
  setUserName: (name: string | null) => void;
  setRole: (role: string | null) => void;
  logout: () => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [userId, setUserIdState] = useState<string | null>(null);
  const [userName, setUserNameState] = useState<string | null>(null);
  const [role, setRoleState] = useState<string | null>(null);

  // Load session from localStorage on app start
  useEffect(() => {
    const storedUserId = localStorage.getItem("userId");
    const storedUserName = localStorage.getItem("userName");
    const storedRole = localStorage.getItem("role");

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

  const logout = () => {
    setUserId(null);
    setUserName(null);
    setRole(null);
    localStorage.clear(); // Clear all session data
  };

  return (
    <UserContext.Provider
      value={{
        userId,
        userName,
        role,
        setUserId,
        setUserName,
        setRole,
        logout,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};

export const useUser = (): UserContextType => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useUser must be used within a UserProvider");
  }
  return context;
};
