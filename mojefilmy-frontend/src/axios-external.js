// axios-external.js
import axios from 'axios';

const axiosExternalInstance = axios.create({
  baseURL: 'http://localhost:8080' // Zamiast bezpośredniego URL zewnętrznego API
});

export default axiosExternalInstance;