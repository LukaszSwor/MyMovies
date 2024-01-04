import axiosApi from '@/axios-api';
import axiosExternal from '@/axios-external';

const fetchMovies = () => axiosApi.get('/movies').then(response => response.data);

const fetchExternalMovies = () => axiosExternal.get('/MyMovies').then(response => response.data);

const addNewMovie = movie => axiosApi.post('/movies', movie).then(response => response.data);

const updateMovie = (id, movie) => axiosApi.put(`/movies/${id}`, movie);

const deleteMovie = id => axiosApi.delete(`/movies/${id}`);

const checkIfExists = (title, director) => 
axiosApi.get(`/movies/checkIfExists?title=${encodeURIComponent(title)}&
director=${encodeURIComponent(director)}`).then(response => response.data);

export default {
  fetchMovies,
  fetchExternalMovies,
  addNewMovie,
  updateMovie,
  deleteMovie,
  checkIfExists
};
