import React, { createContext, useContext, useState, useEffect, ReactNode } from "react";

interface UserContextType {
  userId: string | null;
  userName: string | null;
  setUserId: (id: string | null) => void;
  setUserName: (name: string | null) => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [userId, setUserIdState] = useState<string | null>(null);
  const [userName, setUserNameState] = useState<string | null>(null);

  // Load userId from localStorage on initial render
  useEffect(() => {
    const storedUserId = localStorage.getItem("userId");
    const storedUserNamne = localStorage.getItem("userName");
    if (storedUserId) {
      setUserIdState(storedUserId);
    }
    if (storedUserNamne) {
      setUserNameState(storedUserNamne);
    }
  }, []);

  // Save userId to localStorage whenever it changes
  const setUserId = (id: string | null) => {
    if (id) {
      localStorage.setItem("userId", id);
    } else {
      localStorage.removeItem("userId");
    }
    setUserIdState(id);
  };

  const setUserName = (name: string | null) => {
    if (name) {
      localStorage.setItem("userName", name);
    } else {
      localStorage.removeItem("userName");
    }
    setUserNameState(name);
  };

  return (
    <UserContext.Provider value={{ userId, setUserId, userName, setUserName }}>
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
