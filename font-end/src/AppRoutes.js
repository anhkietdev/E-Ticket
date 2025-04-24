import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';

// Auth components
import { StaffRoute, UserRoute, AuthRoute } from './components/ProtectedRoute';
import { useAuth } from './context/AuthContext';

// Authentication pages
import LoginPage from './pages/LoginPage';
import StaffLoginPage from './pages/StaffLoginPage';

// User pages
import HomePage from './pages/HomePage';
import MoviesPage from './pages/MoviesPage';
import MovieDetailPage from './pages/MovieDetailPage';
import BookingPage from './pages/BookingPage';
import UserProfilePage from './pages/UserProfilePage';

// Staff pages
import StaffLayout from './components/StaffLayout';
import StaffDashboard from './pages/StaffDashboard';
import FilmsManagementPage from './pages/FilmsManagementPage';
import FilmForm from './pages/FilmForm';
import GenresManagementPage from './pages/GenresManagementPage';
import ProjectionsManagementPage from './pages/ProjectionsManagementPage';
import ProjectionForm from './pages/ProjectionForm';
import BookingsManagementPage from './pages/BookingsManagementPage';

const AppRoutes = () => {
  const { currentUser, userRole } = useAuth();

  return (
    <Routes>
      {/* Public routes (no authentication required) */}
      <Route path="/" element={<HomePage />} />
      <Route path="/movies" element={<MoviesPage />} />
      <Route path="/movie/:movieId" element={<MovieDetailPage />} />
      
      {/* Authentication routes */}
      <Route 
        path="/login" 
        element={
          currentUser ? (
            userRole === 'staff' ? <Navigate to="/staff" /> : <Navigate to="/" />
          ) : <LoginPage />
        } 
      />
      
      <Route 
        path="/staff/login" 
        element={
          currentUser && userRole === 'staff' ? 
            <Navigate to="/staff" /> : 
            <StaffLoginPage />
        } 
      />

      {/* User-only routes */}
      <Route element={<UserRoute />}>
        <Route path="/booking/:movieId" element={<BookingPage />} />
        <Route path="/profile" element={<UserProfilePage />} />
      </Route>

      {/* Staff-only routes */}
      <Route path="/staff" element={<StaffRoute />}>
        <Route element={<StaffLayout />}>
          <Route index element={<StaffDashboard />} />
          
          {/* Film Management Routes */}
          <Route path="films" element={<FilmsManagementPage />} />
          <Route path="films/add" element={<FilmForm mode="add" />} />
          <Route path="films/edit/:id" element={<FilmForm mode="edit" />} />
          
          {/* Genre Management Routes */}
          <Route path="genres" element={<GenresManagementPage />} />
          
          {/* Projection Management Routes */}
          <Route path="projections" element={<ProjectionsManagementPage />} />
          <Route path="projections/add" element={<ProjectionForm mode="add" />} />
          <Route path="projections/edit/:id" element={<ProjectionForm mode="edit" />} />
          
          {/* Booking Management Routes */}
          <Route path="bookings" element={<BookingsManagementPage />} />
        </Route>
      </Route>

      {/* Redirect routes */}
      <Route path="/staff/*" element={<Navigate to="/staff/login" replace />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
};

export default AppRoutes;