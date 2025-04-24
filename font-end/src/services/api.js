import axios from 'axios';

// Base configuration for axios
const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5086';

// Create axios instance
const api = axios.create({
  baseURL: API_URL,
  timeout: 10000,
});

// Add token to request headers
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Common error handler
const handleError = (error) => {
  if (error.response) {
    console.error('API Error Response:', error.response.data);
    throw new Error(error.response.data.message || 'An error occurred');
  } else if (error.request) {
    console.error('API Error Request:', error.request);
    throw new Error('Cannot connect to server');
  } else {
    console.error('API Error:', error.message);
    throw new Error('An error occurred while sending the request');
  }
};

// Movie Service
export const movieService = {
  // Get all movies
  getAll: async () => {
    try {
      const response = await api.get('/api/Film');
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Get movie by ID
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Film/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Search movie by title
  getByTitle: async (title) => {
    try {
      const response = await api.get(`/api/Film/by-title/${title}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Staff-only endpoints
  create: async (filmData) => {
    try {
      const response = await api.post('/api/Film', filmData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  update: async (id, filmData) => {
    try {
      const response = await api.put(`/api/Film/${id}`, filmData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  delete: async (id) => {
    try {
      const response = await api.delete(`/api/Film/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  }
};

// Create alias for film service
export const filmService = movieService;

// Genre Service
export const genreService = {
  // Get all genres
  getAll: async () => {
    try {
      const response = await api.get('/api/Genre');
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Get genre by ID
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Genre/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Staff-only endpoints
  create: async (genreData) => {
    try {
      const response = await api.post('/api/Genre', genreData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  update: async (id, genreData) => {
    try {
      const response = await api.put(`/api/Genre/${id}`, genreData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  delete: async (id) => {
    try {
      const response = await api.delete(`/api/Genre/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  }
};

// Room Service
export const roomService = {
  // Get all rooms
  getAll: async () => {
    try {
      const response = await api.get('/api/Room');
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Get room by ID
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Room/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Staff-only endpoints
  create: async (roomData) => {
    try {
      const response = await api.post('/api/Room', roomData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  update: async (id, roomData) => {
    try {
      const response = await api.put(`/api/Room/${id}`, roomData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  delete: async (id) => {
    try {
      const response = await api.delete(`/api/Room/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  }
};

// Projection Service
export const projectionService = {
  // Get all projections
  getAll: async () => {
    try {
      const response = await api.get('/api/Projection');
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Get projection by ID
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Projection/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Get projections by film
  getByFilmId: async (filmId) => {
    try {
      const response = await api.get(`/api/Projection/by-film/${filmId}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Staff-only endpoints
  create: async (projectionData) => {
    try {
      const response = await api.post('/api/Projection', projectionData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  update: async (id, projectionData) => {
    try {
      const response = await api.put(`/api/Projection/${id}`, projectionData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  delete: async (id) => {
    try {
      const response = await api.delete(`/api/Projection/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  }
};

// Create alias for showTimeService
export const showTimeService = projectionService;

// Seat Service
export const seatService = {
  // Get all seats
  getAll: async () => {
    try {
      const response = await api.get('/api/Seat');
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Get seat by ID
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Seat/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Get available seats by projection
  getAvailableByProjectionId: async (projectionId) => {
    try {
      const response = await api.get(`/api/Seat/available-by-projection/${projectionId}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  }
};

// Authentication Service
export const authService = {
  // User login
  login: async (credentials) => {
    try {
      const response = await api.post('/Authentication/Login/login', credentials);
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Staff login (separate endpoint)
  staffLogin: async (credentials) => {
    try {
      const response = await api.post('/Authentication/Staff/login', credentials);
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // User registration
  register: async (userData) => {
    try {
      const response = await api.post('/api/User/register', userData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Get current user info
  getCurrentUser: async () => {
    try {
      const userId = localStorage.getItem('userId');
      if (!userId) {
        throw new Error('User ID not found');
      }
      const response = await api.get(`/api/User/${userId}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Logout (client-side only)
  logout: async () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('userRole');
    localStorage.removeItem('staffUser');
    return Promise.resolve({ success: true });
  }
};

// Booking Service
export const bookingService = {
  // Create booking
  create: async (bookingData) => {
    try {
      const response = await api.post('/api/Ticket', bookingData);
      return response;
    } catch (error) {
      handleError(error);
    }
  },

  // Get ticket details
  getTicketDetails: async (projectionId, seatIds) => {
    try {
      const response = await api.get(`/api/Ticket/details/${projectionId}`, {
        params: { seatIds: seatIds.join(',') }
      });
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  // Staff-only endpoints
  getAll: async () => {
    try {
      const response = await api.get('/api/Ticket');
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  getById: async (id) => {
    try {
      const response = await api.get(`/api/Ticket/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  },
  
  delete: async (id) => {
    try {
      const response = await api.delete(`/api/Ticket/${id}`);
      return response;
    } catch (error) {
      handleError(error);
    }
  }
};

// ZaloPay Service
export const paymentService = {
  // Create ZaloPay payment
  createPayment: async (paymentData) => {
    try {
      const response = await api.post('/Zalopay/CreatePayment', paymentData);
      return response;
    } catch (error) {
      handleError(error);
    }
  }
};

// Export default for easy importing
export default {
  movieService,
  filmService,
  genreService,
  roomService,
  projectionService,
  showTimeService,
  seatService,
  authService,
  bookingService,
  paymentService
};