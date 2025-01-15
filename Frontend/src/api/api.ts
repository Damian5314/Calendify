import axios from "axios";

// Base URL for the API
const BASE_URL = "http://localhost:5097/api/v1";

// Create an Axios instance with default settings
const api = axios.create({
  baseURL: BASE_URL,
  withCredentials: true, // Include session cookies
});

// Exported API functions

// Authentication APIs
export const login = (email: string, password: string) =>
  api.post("/auth/login", { email, password });

export const registerUser = (data: any) => api.post("/auth/register", data);

export const registerAdmin = (data: any) =>
  api.post("/auth/register-admin", data);

export const getSessionRole = () => api.get("/auth/is-logged-in");

export const logout = () => api.post("/auth/logout");

// User Info APIs

// Fetch all user info at once
export const fetchUserInfo = () => api.get("/auth/user-info");

// Fetch individual user details
export const fetchFirstName = () =>
  api.get("/auth/user-info").then((res) => res.data.firstName);
export const fetchLastName = () =>
  api.get("/auth/user-info").then((res) => res.data.lastName);
export const fetchEmail = () =>
  api.get("/auth/user-info").then((res) => res.data.email);
export const fetchRecurringDays = () =>
  api.get("/auth/user-info").then((res) => res.data.recurringDays);

// Update User Data APIs

// Update email
export const updateEmail = (email: string) =>
  api.put("/auth/update-email", { email });

// Update password
export const updatePassword = (currentPassword: string, newPassword: string) =>
  api.put("/auth/update-password", { currentPassword, newPassword });

// Update first name
export const updateFirstName = (firstName: string) =>
  api.put("/auth/update-firstname", { firstName });

// Update last name
export const updateLastName = (lastName: string) =>
  api.put("/auth/update-lastname", { lastName });

// Update recurring days
export const updateRecurringDays = (recurringDays: string) =>
  api.put("/auth/update-recurringdays", { recurringDays });

export default api;
