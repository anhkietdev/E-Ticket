
import api from './api';

const movieService = {
  getAll: () => api.get('/Film'),
  getById: (id) => api.get(`/Film/${id}`),
  getNowPlaying: () => api.get('/Film?releaseDate_lte=2025-04-17'),
  getUpcoming: () => api.get('/Film?releaseDate_gt=2025-04-17'),
  searchMovies: (query) => api.get(`/Film?q=${query}`),
  getByGenre: (genre) => api.get(`/Film?genre_like=${genre}`),
};

export default movieService;