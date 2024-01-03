<!-- HomePage.vue -->
<template>
  <div class="container mt-5">
    <h1>Moja Biblioteka Filmowa</h1>
    <p>Witaj w mojej aplikacji do zarządzania biblioteką filmową.</p>

    <!-- Przycisk do pobierania filmów z API -->
    <button @click="fetchAndShowMoviesFromAPI" class="btn btn-primary mb-3">
      Pobierz filmy
    </button>

    <!-- Przycisk do dodawania nowych filmów -->
    <button @click="showAddMovieModal" class="btn btn-primary mb-3">
      Dodaj własny film
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

    <!-- Lista filmów w lokalnej bibliotece -->
    <movie-list
      :movies="filteredMovies"
      @edit-movie="editMovie"
      @delete-movie="deleteMovie"
    />
  </div>

  <!-- Okno modalne do dodawania/edycji filmów -->
  <movie-modal
    :is-visible="isEditMovieModalVisible"
    @close="closeEditMovieModal"
  >
    <movie-form
      :movie-data="editingMovie"
      @save-movie="saveMovie"
      @update-movie="updateMovie" 
    />
  </movie-modal>

  <!-- Okno modalne do wyświetlania filmów z zewnętrznego API -->
  <movie-modal
    :movies="externalMovies"
    :is-visible="isExternalMoviesModalVisible"
    @close="isExternalMoviesModalVisible = false"
    @add-movie="addMovieToLocalLibrary"
  />
</template>

<script>
import MovieList from './MovieList.vue';
import MovieForm from './MovieForm.vue';
import MovieModal from '../components/MovieModal.vue';

import axiosApi from '@/axios-api'; // Dla lokalnego API
import axiosExternal from '@/axios-external'; // Dla zewnętrznego API

export default {
  components: {
    MovieList,
    MovieForm,
    MovieModal
  },
  data() {
    return {
      movies: [], // Lokalna lista filmów
      externalMovies: [], // Filmy pobrane z zewnętrznego API
      editingMovie: null, // Film wybrany do edycji
      searchQuery: '', // Zapytanie do wyszukiwarki
      isEditMovieModalVisible: false, // Widoczność okna modalnego do edycji filmu
      isExternalMoviesModalVisible: false // Widoczność okna modalnego dla filmów zewnętrznych
    };
  },
  computed: {
    filteredMovies() {
      if (!this.searchQuery) {
        return this.movies;
      }
      const searchLowerCase = this.searchQuery.toLowerCase();
      return this.movies.filter(movie => {
        return movie.title.toLowerCase().includes(searchLowerCase) ||
               movie.director.toLowerCase().includes(searchLowerCase) ||
               movie.year.toString().includes(searchLowerCase);
      });
    }
  },
  mounted() {
    this.fetchMovies();
  },
    methods: {
    fetchAndShowMoviesFromAPI() {
      axiosExternal.get('/MyMovies')
        .then(response => {
          this.externalMovies = response.data;
          this.isExternalMoviesModalVisible = true; // Otwieranie okna modalnego z filmami
        })
        .catch(error => {
          console.error('Error fetching movies from external API:', error);
          this.isExternalMoviesModalVisible = false;
        });
    },
    addMovieToLocalLibrary(movie) {
      // Sprawdzenie, czy film już istnieje w lokalnej bazie danych
      axiosApi.get(`/movies/checkIfExists?title=${encodeURIComponent(movie.title)}&director=${encodeURIComponent(movie.director)}`)
        .then(response => {
          if (!response.data) {
            // Jeśli film nie istnieje, dodaj go do lokalnej bazy danych
            axiosApi.post('/movies', movie)
              .then(response => {
                this.movies.push(response.data);
                this.sortMovies('title');
              })
              .catch(error => {
                console.error('Error adding movie to local library:', error);
              });
          }
        })
        .catch(error => {
          console.error('Error checking if movie exists:', error);
        });
    },
    fetchMovies() {
    axiosApi.get('/movies')
      .then(response => {
        this.movies = response.data; // Aktualizacja listy filmów danymi z serwera
                this.sortMovies('title'); // Dodaj sortowanie po pobraniu filmów

      })
      .catch(error => {
        console.error('Error fetching movies:', error);
      });
  },
    
      saveMovie(movie) {
    if (movie.id) {
      this.updateMovie(movie);
    } else {
      this.addNewMovie(movie);
    }
    this.sortMovies('title');
    this.isEditMovieModalVisible = false; // Zamykanie modalu po zapisaniu
  },
    updateMovie(updatedMovie) {
    axiosApi.put(`/movies/${updatedMovie.id}`, updatedMovie)
      .then(() => {
        this.isEditMovieModalVisible = false; // Zamknij modal po aktualizacji
        this.editingMovie = null; // Resetowanie edytowanego filmu
                this.fetchMovies(); // Ponownie pobieranie listy filmów

      })
      .catch(error => {
        console.error('Error updating movie:', error);
      });
  },
  addNewMovie(movie) {
    axiosApi.post('/movies', movie)
      .then(response => {
         this.movies.push(response.data);
        this.sortMovies('title'); // Dodaj sortowanie tuż po dodaniu filmu
        this.isEditMovieModalVisible = false;
      })
      .catch(error => {
        console.error('Error adding new movie:', error);
      });
  },
        showAddMovieModal() {
      this.editingMovie = { title: '', director: '', year: null }; // Resetuje formularz
      this.isEditMovieModalVisible = true; // Pokazuje modal do dodawania filmu
    },
    closeEditMovieModal() {
      this.isEditMovieModalVisible = false;
    },
      editMovie(movie) {
    this.editingMovie = { ...movie };
    this.isEditMovieModalVisible = true; // Otwieranie okna modalnego do edycji
  },
    deleteMovie(id) {
      axiosApi.delete(`/movies/${id}`)
        .then(() => {
          this.movies = this.movies.filter(m => m.id !== id);
        })
        .catch(error => {
          console.error('Error deleting movie:', error);
        });
    },
    sortMovies(key) {
    if (key === 'title') {
      // Sortowanie tytułów z uwzględnieniem wielkości liter i lokalnych znaków
      this.movies.sort((a, b) => a[key].localeCompare(b[key], 'pl', { sensitivity: 'base' }));
    } else {
      // Domyślne sortowanie dla innych kluczy (np. 'year')
      this.movies.sort((a, b) => (a[key] > b[key]) ? 1 : ((b[key] > a[key]) ? -1 : 0));
    }
  },
    showMovieModal(movie) {
      console.log('Dodaj do biblioteki')
      this.selectedMovie = movie;
      this.isModalVisible = true;
    }
  }
};
</script>


<style>
/* Tutaj dodaj swoje style */
</style>