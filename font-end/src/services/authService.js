import api from './api';

const authService = {
  login: async (credentials) => {
    try {
      const response = await api.post('/Authentication/login/login', credentials);
      const { token, user } = response.data;

      if (!token || !user) {
        throw new Error('Invalid response from server');
      }

      localStorage.setItem('token', token);
      localStorage.setItem('userId', user.id);
      return { token, user };
    } catch (error) {
      throw new Error(error.response?.data || 'Đăng nhập thất bại');
    }
  },

  register: async (userData) => {
    try {
      const response = await api.post('/Authentication/register', userData);
      const { token, user } = response.data;

      if (!token || !user) {
        throw new Error('Invalid response from server');
      }

      localStorage.setItem('token', token);
      localStorage.setItem('userId', user.id);
      return { token, user };
    } catch (error) {
      throw new Error(error.response?.data || 'Đăng ký thất bại');
    }
  },

  getCurrentUser: async () => {
    try {
      const userId = localStorage.getItem('userId');
      if (!userId) {
        throw new Error('Không có người dùng đăng nhập');
      }
      const response = await api.get(`/users/${userId}`);
      return response;
    } catch (error) {
      throw new Error('Không thể lấy thông tin người dùng');
    }
  },

  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    return Promise.resolve();
  },
};

export default authService;