<!-- HomePage.vue -->
<template>
  <div class="container mt-5">
    <h1>Moja Biblioteka Filmowa</h1>
    <p>Witaj w mojej aplikacji do zarządzania biblioteką filmową.</p>

    <!-- Przycisk do pobierania filmów z API -->
    <button @click="fetchMoviesFromAPI" class="btn btn-primary mb-3">
      Pobierz filmy
    </button>

    <!-- Wyszukiwanie i sortowanie filmów -->
    <input
      type="text"
      v-model="searchQuery"
      placeholder="Wyszukaj..."
      class="form-control mb-3"
    />
    <button @click="sortMovies('title')" class="btn btn-secondary">
      Sortuj wg Tytułu
    </button>
    <button @click="sortMovies('year')" class="btn btn-secondary">
      Sortuj wg Roku
    </button>

    <!-- Lista filmów pobranych z API -->
    <div v-if="externalMovies.length > 0">
      <h2>Filmy z zewnętrznego API:</h2>
      <ul>
        <li v-for="movie in externalMovies" :key="movie.id">
          {{ movie.title }} ({{ movie.year }}) - Reżyseria: {{ movie.director }}
          <button
            @click="addMovieToLocalLibrary(movie)"
            class="btn btn-success"
          >
            Dodaj do biblioteki
          </button>
        </li>
      </ul>
    </div>

    <!-- Formularz do dodawania/edycji filmów -->
    <movie-form :movie-data="editingMovie" @save-movie="saveMovie" />

    <!-- Lista filmów w lokalnej bibliotece -->
    <movie-list
      :movies="movies"
      @edit-movie="editMovie"
      @delete-movie="deleteMovie"
    />
  </div>
</template>

<script>
import axios from 'axios';
import MovieList from './MovieList.vue';
import MovieForm from './MovieForm.vue';

export default {
  components: {
    MovieList,
    MovieForm
  },
  data() {
    return {
      movies: [], // Lokalna lista filmów
      externalMovies: [], // Filmy pobrane z API
      editingMovie: null, // Film wybrany do edycji
      searchQuery: '', // Zapytanie do wyszukiwarki
    };
  },
  methods: {
    fetchMoviesFromAPI() {
      console.log('Rozpoczęcie pobierania filmów z API...');
      axios.get('/MyMovies') // Używamy skonfigurowanego proxy
        .then(response => {
          console.log('Filmy zostały pobrane:', response.data);
          this.externalMovies = response.data;
        })
        .catch(error => {
          console.error('Wystąpił błąd podczas pobierania filmów:', error);
        });
    },
    addMovieToLocalLibrary(movie) { // Tutaj brakowało przecinka
      const exists = this.movies.some(m => m.id === movie.id);
      if (!exists) {
        this.movies.push(movie);
        console.log('Film dodany do lokalnej biblioteki:', movie);
      }
    },
    saveMovie(newMovie) {
      const existingIndex = this.movies.findIndex(m => m.id === newMovie.id);
      if (existingIndex !== -1) {
        this.movies.splice(existingIndex, 1, newMovie);
      } else {
        newMovie.id = Date.now(); // Przykładowe unikalne ID
        this.movies.push(newMovie);
      }
      this.editingMovie = null;
      console.log('Zapisano film:', newMovie);
    },
    editMovie(movie) {
      this.editingMovie = { ...movie };
      console.log('Edycja filmu:', movie);
    },
    deleteMovie(id) {
      this.movies = this.movies.filter(m => m.id !== id);
      console.log('Film usunięty z lokalnej biblioteki:', id);
    },
    sortMovies(key) {
      this.movies.sort((a, b) => (a[key] > b[key]) ? 1 : ((b[key] > a[key]) ? -1 : 0));
      console.log(`Filmy posortowane po ${key}`);
    }
  }
};
</script>

<style>
/* Tutaj dodaj swoje style */
</style>
