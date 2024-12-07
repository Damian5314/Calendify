import axios from "axios";

const API = axios.create({
  baseURL: "http://localhost:5097/api/v1", // Backend URL
  withCredentials: true, // Ensures cookies are sent
});

export default API;
