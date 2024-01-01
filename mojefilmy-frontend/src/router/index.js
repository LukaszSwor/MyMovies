import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '../views/HomePage.vue';

const routes = [
  {
    path: '/',
    name: 'HomePage', // Tutaj też powinno być 'HomePage' zamiast 'Home'
    component: HomePage // i tutaj odwołanie do importowanego komponentu HomePage
  },
  // Definicje innych ścieżek...
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;