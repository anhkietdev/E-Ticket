/*import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import { BookingProvider } from './context/BookingContext';

// Pages
import HomePage from './pages/HomePage';
import MoviesPage from './pages/MoviesPage';
import MovieDetailPage from './pages/MovieDetailPage';
import SeatSelectionPage from './pages/SeatSelectionPage';
import CheckoutPage from './pages/CheckoutPage';
import LoginPage from './pages/LoginPage';
import ProfilePage from './pages/ProfilePage';
import Navbar from './components/common/Navbar';
// Protected Route Component
const ProtectedRoute = ({ children }) => {
  const { currentUser, loading } = useAuth();
  
  if (loading) {
    return <div>Loading...</div>;
  }
  
  if (!currentUser) {
    return <Navigate to="/login" />;
  }
  
  return children;
};

function App() {
  return (
    <Router>
      <AuthProvider>
        <BookingProvider>
          <Navbar/>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/movies" element={<MoviesPage />} />
            <Route path="/movies/:id" element={<MovieDetailPage />} />
            
            <Route 
              path="/booking/seats" 
              element={
                <ProtectedRoute>
                  <SeatSelectionPage />
                </ProtectedRoute>
              } 
            />
            <Route 
              path="/booking/checkout" 
              element={
                <ProtectedRoute>
                  <CheckoutPage />
                </ProtectedRoute>
              } 
            />
            <Route path="/login" element={<LoginPage />} />
            <Route 
              path="/profile" 
              element={
                <ProtectedRoute>
                  <ProfilePage />
                </ProtectedRoute>
              } 
            />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
        </BookingProvider>
      </AuthProvider>
    </Router>
  );
}

export default App;*/

import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import  {AuthProvider}  from './context/AuthContext';
import  {BookingProvider}  from './context/BookingContext';

// Pages
import HomePage from './pages/HomePage';
import MoviesPage from './pages/MoviesPage';
import MovieDetailPage from './pages/MovieDetailPage';
import LoginPage from './pages/LoginPage';
import Navbar from './components/common/Navbar';

// Staff pages
import StaffLayout from './pages/staff/StaffLayout';
import StaffDashboard from './pages/staff/StaffDashboard';
import FilmsManagementPage from './pages/staff/FilmsManagementPage';
import FilmForm from './pages/staff/FilmForm';
import GenresManagementPage from './pages/staff/GenresManagementPage';
import ProjectionsManagementPage from './pages/staff/ProjectionsManagementPage';
import ProjectionForm from './pages/staff/ProjectionsManagementPage';
import BookingsManagementPage from './pages/staff/BookingsManagementPage';

function App() {
  return (
    <Router>
      <AuthProvider>
        <BookingProvider>
          <Navbar/>
          <Routes>
            {/* User routes */}
            <Route path="/" element={<HomePage />} />
            <Route path="/movies" element={<MoviesPage />} />
            <Route path="/movies/:id" element={<MovieDetailPage />} />
            <Route path="/login" element={<LoginPage />} />
      
           
            <Route path="/staff" element={<StaffLayout />}>
              <Route index element={<StaffDashboard />} />
              <Route path="films" element={<FilmsManagementPage />} />
              <Route path="films/add" element={<FilmForm mode="add" />} />
              <Route path="films/edit/:id" element={<FilmForm mode="edit" />} />
              <Route path="genres" element={<GenresManagementPage />} />
              <Route path="projections" element={<ProjectionsManagementPage />} />
              <Route path="projections/add" element={<ProjectionForm mode="add" />} />
              <Route path="projections/edit/:id" element={<ProjectionForm mode="edit" />} />
              <Route path="bookings" element={<BookingsManagementPage />} />
            </Route>
            
            
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
        </BookingProvider>
      </AuthProvider>
    </Router>
  );
}

export default App;