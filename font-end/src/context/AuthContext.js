import React, { createContext, useContext, useState, useEffect } from 'react';
import { authService } from '../services/api';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);
  const [userRole, setUserRole] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadUser = async () => {
      try {
        const token = localStorage.getItem('token');
        const storedRole = localStorage.getItem('userRole');
        
        if (token) {
          if (storedRole === 'staff') {
            // For staff, we use data stored in localStorage
            const staffUser = JSON.parse(localStorage.getItem('staffUser') || '{}');
            setCurrentUser(staffUser);
            setUserRole('staff');
          } else {
            // For regular users, fetch from API
            const response = await authService.getCurrentUser();
            setCurrentUser(response.data);
            setUserRole('user');
          }
        }
      } catch (err) {
        console.error('Không thể tải thông tin người dùng:', err);
        setError('Không thể xác thực người dùng. Vui lòng đăng nhập lại.');
        logout(); // Clear all auth data
      } finally {
        setLoading(false);
      }
    };

    loadUser();
  }, []);

  // Regular user login
  const login = async (email, password) => {
    try {
      setError(null);
      const response = await authService.login({ email, password });
      const { user, token } = response.data;
      
      localStorage.setItem('token', token);
      localStorage.setItem('userId', user.id);
      localStorage.setItem('userRole', 'user');
      
      setCurrentUser(user);
      setUserRole('user');
      
      return user;
    } catch (err) {
      setError(err.message || 'Email hoặc mật khẩu không đúng');
      throw err;
    }
  };

  // Staff login
  const staffLogin = async (username, password) => {
    try {
      setError(null);
      
      // In a real app, you would call the API
      // For this example, we'll use hardcoded credentials
      if (username === 'admin' && password === 'admin123') {
        const staffUser = {
          id: 'staff-1',
          username: username,
          name: 'Admin',
          role: 'staff'
        };
        
        // Generate fake token for staff
        const staffToken = 'staff-token-' + Date.now();
        
        localStorage.setItem('token', staffToken);
        localStorage.setItem('userRole', 'staff');
        localStorage.setItem('staffUser', JSON.stringify(staffUser));
        
        setCurrentUser(staffUser);
        setUserRole('staff');
        
        return staffUser;
      } else {
        throw new Error('Thông tin đăng nhập không hợp lệ');
      }
    } catch (err) {
      setError(err.message || 'Đăng nhập thất bại');
      throw err;
    }
  };

  // User registration
  const register = async (userData) => {
    try {
      setError(null);
      const response = await authService.register(userData);
      const { user, token } = response.data;
      
      localStorage.setItem('token', token);
      localStorage.setItem('userId', user.id);
      localStorage.setItem('userRole', 'user');
      
      setCurrentUser(user);
      setUserRole('user');
      
      return user;
    } catch (err) {
      setError(err.message || 'Đăng ký thất bại');
      throw err;
    }
  };

  // Logout for both user and staff
  const logout = () => {
    // Clear all localStorage items
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('userRole');
    localStorage.removeItem('staffUser');
    
    setCurrentUser(null);
    setUserRole(null);
    setError(null);
  };

  // Check if user is staff
  const isStaff = () => userRole === 'staff';
  
  // Check if user is regular user
  const isUser = () => userRole === 'user';

  const value = {
    currentUser,
    userRole,
    login,
    staffLogin,
    register,
    logout,
    loading,
    error,
    isStaff,
    isUser,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthContext;