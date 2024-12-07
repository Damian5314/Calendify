import axios from "axios";

const BASE_URL = "http://localhost:5097/api/v1";

const api = axios.create({
  baseURL: BASE_URL,
  withCredentials: true, // Include session cookies
});

export const login = (email: string, password: string) =>
  api.post("/auth/login", { email, password });

export const registerUser = (data: any) => api.post("/auth/register", data);

export const registerAdmin = (data: any) =>
  api.post("/auth/register-admin", data);

export const getSessionRole = () => api.get("/auth/is-logged-in");

export const fetchAllUsers = () => api.get("/admins/users");

export const deleteUser = (id: number) => api.delete(`/admins/users/${id}`);

export const updateUser = (id: number, data: any) =>
  api.put(`/admins/users/${id}`, data);
