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
  recuringDays: string[];
  setUserId: (id: number | null) => void;
  setUserName: (name: string | null) => void;
  setRole: (role: string | null) => void;
  setRecuringDays: (days: string[]) => void;
  logout: () => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [userId, setUserIdState] = useState<number | null>(null);
  const [userName, setUserNameState] = useState<string | null>(null);
  const [role, setRoleState] = useState<string | null>(null);
  const [recuringDays, setRecuringDaysState] = useState<string[]>([]);

  // 🔹 Load user session from localStorage on app startup
  useEffect(() => {
    const storedUserId = localStorage.getItem("userId");
    const storedUserName = localStorage.getItem("userName");
    const storedRole = localStorage.getItem("role");
    const storedDays = localStorage.getItem("recuringDays");

    if (storedUserId) setUserIdState(parseInt(storedUserId, 10));
    if (storedUserName) setUserNameState(storedUserName);
    if (storedRole) setRoleState(storedRole);
    if (storedDays) {
      try {
        // 🔹 Zorg ervoor dat alles correct wordt verwerkt
        const parsedDays = JSON.parse(storedDays);
        if (Array.isArray(parsedDays)) {
          setRecuringDaysState(parsedDays.map((day: string) => day.trim()));
        }
      } catch (error) {
        console.error("Error parsing recurring days from localStorage:", error);
        setRecuringDaysState([]); // Fallback als er een fout is
      }
    }
  }, []);

  // 🔹 Store session in localStorage on changes
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

  const setRecuringDays = (days: string[]) => {
    try {
      const formattedDays = days.map((day) => day.trim());
      localStorage.setItem("recuringDays", JSON.stringify(formattedDays));
      setRecuringDaysState(formattedDays);
    } catch (error) {
      console.error("Error saving recurring days to localStorage:", error);
    }
  };

  const logout = () => {
    localStorage.removeItem("userId");
    localStorage.removeItem("userName");
    localStorage.removeItem("role");
    localStorage.removeItem("recuringDays");
    setUserIdState(null);
    setUserNameState(null);
    setRoleState(null);
    setRecuringDaysState([]);
  };

  return (
    <UserContext.Provider
      value={{
        userId,
        userName,
        role,
        recuringDays,
        setUserId,
        setUserName,
        setRole,
        setRecuringDays,
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
