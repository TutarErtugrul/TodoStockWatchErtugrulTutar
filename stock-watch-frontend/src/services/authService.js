import axios from "axios";
import jwt_decode from "jwt-decode";

const API_URL = "https://localhost:7038/api/auth";

export function getCurrentUser() {
    const user = localStorage.getItem("user");
    return user ? JSON.parse(user) : null;
  }
  
export const login = async (username, password) => {
  try {
    const response = await axios.post(`${API_URL}/login`, { username, password });
    if (response.data.token) {
      localStorage.setItem("userToken", response.data.token);
    }
    return response.data;
  } catch (error) {
    throw new Error("Giriş başarısız! Lütfen bilgilerinizi kontrol edin.");
  }
};

export const logout = () => {
  localStorage.removeItem("userToken");
};

export const getToken = () => {
  return localStorage.getItem("userToken");
};
export const isTokenExpired = () => {
  const token = getToken();

  if (!token) {
    return true; 
  }

  const decodedToken = jwt_decode(token);
  return Date.now() >= decodedToken.exp * 1000;
};
