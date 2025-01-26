import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";

interface UserContextType {
  userId: number | null;
  userName: string | null;
  role: string | null;
  setUserId: (id: number | null) => void;
  setUserName: (name: string | null) => void;
  setRole: (role: string | null) => void;
  logout: () => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [userId, setUserIdState] = useState<number | null>(null);
  const [userName, setUserNameState] = useState<string | null>(null);
  const [role, setRoleState] = useState<string | null>(null);

  // Load user data from localStorage
  useEffect(() => {
    const storedUserId = localStorage.getItem("userId");
    const storedUserName = localStorage.getItem("userName");
    const storedRole = localStorage.getItem("role");

    if (storedUserId) setUserIdState(parseInt(storedUserId, 10));
    if (storedUserName) setUserNameState(storedUserName);
    if (storedRole) setRoleState(storedRole);

    // Listen for changes in localStorage to synchronize logout across tabs
    const handleStorageChange = (event: StorageEvent) => {
      if (event.key === "logout" && event.newValue === "true") {
        setUserIdState(null);
        setUserNameState(null);
        setRoleState(null);
        localStorage.clear(); // Clear all localStorage data
      }
    };

    window.addEventListener("storage", handleStorageChange);

    return () => {
      window.removeEventListener("storage", handleStorageChange);
    };
  }, []);

  const setUserId = (id: number | null) => {
    if (id !== null) localStorage.setItem("userId", id.toString());
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
    localStorage.clear();
    localStorage.setItem("logout", "true"); // Trigger storage event
    setTimeout(() => localStorage.removeItem("logout"), 0); // Cleanup
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
  if (!context) throw new Error("useUser must be used within a UserProvider");
  return context;
};
