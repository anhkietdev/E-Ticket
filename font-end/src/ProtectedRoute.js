import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import LoadingSpinner from '../LoadingSpinner';

const ProtectedRoute = ({ children }) => {
  const { currentUser, loading } = useAuth();
  const location = useLocation();

  if (loading) {
    return <LoadingSpinner message="Đang kiểm tra thông tin đăng nhập..." />;
  }

  if (!currentUser) {
    // Lưu lại đường dẫn hiện tại để sau khi đăng nhập có thể quay lại
    return <Navigate to="/login" state={{ from: location.pathname }} />;
  }

  return children;
};

export default ProtectedRoute;