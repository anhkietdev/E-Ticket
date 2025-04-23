import React, { createContext, useContext, useState, useEffect } from 'react';
import authService from '../services/authService'; // Changed to default import

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadUser = async () => {
      try {
        const token = localStorage.getItem('token');
        if (token) {
          const response = await authService.getCurrentUser();
          setCurrentUser(response.data);
        }
      } catch (err) {
        console.error('Không thể tải thông tin người dùng:', err);
        setError('Không thể xác thực người dùng. Vui lòng đăng nhập lại.');
        localStorage.removeItem('token');
        localStorage.removeItem('userId');
      } finally {
        setLoading(false);
      }
    };

    loadUser();
  }, []);

  const login = async (email, password) => {
    try {
      const { user, token } = await authService.login({ email, password });
      setCurrentUser(user);
      setError(null);
      return user;
    } catch (err) {
      setError(err.message || 'Email hoặc mật khẩu không đúng');
      throw err;
    }
  };

  const register = async (userData) => {
    try {
      const { user, token } = await authService.register(userData);
      setCurrentUser(user);
      setError(null);
      return user;
    } catch (err) {
      setError(err.message || 'Đăng ký thất bại');
      throw err;
    }
  };

  const logout = () => {
    authService.logout();
    setCurrentUser(null);
    setError(null);
  };

  const value = {
    currentUser,
    login,
    register,
    logout,
    loading,
    error,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};