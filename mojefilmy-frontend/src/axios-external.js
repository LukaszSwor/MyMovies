import axios from 'axios';

const axiosExternalInstance = axios.create({
  baseURL: 'http://localhost:8080'
});

export default axiosExternalInstance;