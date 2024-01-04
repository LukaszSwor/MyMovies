<template>
  <div class="container mt-5">
    <h1>Moja Biblioteka Filmowa</h1>
    <p>Witaj w mojej aplikacji do zarządzania biblioteką filmową.</p>

    <button @click="fetchExternalMovies" class="btn btn-primary mb-3">
      Pobierz filmy
    </button>

    <button @click="showAddMovieModal" class="btn btn-primary mb-3">
      Dodaj film
    </button>

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

    <movie-list
      :movies="filteredMovies"
      @edit-movie="editMovie"
      @delete-movie="deleteMovie"
    />
  
  <movie-modal
    :is-visible="isEditMovieModalVisible"
    @close="closeEditMovieModal"
  >

    <movie-form
      :movie-data="editingMovie || { title: '', director: '', year: null }"
      @save-movie="saveMovie"
      @update-movie="updateMovie"
    />
  </movie-modal>

  <movie-modal
    :movies="externalMovies"
    :is-visible="isExternalMoviesModalVisible"
    @close="isExternalMoviesModalVisible = false"
    @add-movie="addMovieToLocalLibrary"
  />
    <movie-error :error="error"/>
  </div>
</template>

<script>
import MovieList from './MovieList.vue';
import MovieForm from './MovieForm.vue';
import MovieModal from '@/components/MovieModal.vue';
import MovieError from '@/components/MovieError.vue';
import MovieService from '@/services/MovieService';

export default {
  components: {
    MovieList,
    MovieForm,
    MovieModal,
    MovieError
  },
  data() {
    return {
      movies: [],
      externalMovies: [],
      editingMovie: null,
      searchQuery: '',
      isEditMovieModalVisible: false,
      isExternalMoviesModalVisible: false,
      error: null
    };
  },
  computed: {
    filteredMovies() {
      if (!this.searchQuery) return this.movies;
      const searchLowerCase = this.searchQuery.toLowerCase();
      return this.movies.filter(movie =>
        movie.title.toLowerCase().includes(searchLowerCase) ||
        movie.director.toLowerCase().includes(searchLowerCase) ||
        movie.year.toString().includes(searchLowerCase)
      );
    }
  },
  mounted() {
    this.fetchMovies();
  },
  methods: {
    fetchExternalMovies() {
      MovieService.fetchExternalMovies()
        .then(data => {
          this.externalMovies = data;
            if (!this.error) {
            this.isExternalMoviesModalVisible = true;
            }
        })
    .catch(error => this.handleError(error));
    },

    fetchMovies() {
      MovieService.fetchMovies()
        .then(data => {
          this.movies = data;
          this.sortMovies('title');
        })
        .catch(error => this.handleError(error));
    },

    addMovieToLocalLibrary(movie) {
      MovieService.checkIfExists(movie.title, movie.director)
        .then(exists => {
          if (!exists) {
            this.addNewMovie(movie);
          }
        })
        .catch(error => this.handleError(error));
    },

    saveMovie(movie) {
      if (movie.id) {
        this.updateMovie(movie);
      } else {
        this.addNewMovie(movie);
      }
    },

    updateMovie(updatedMovie) {
      MovieService.updateMovie(updatedMovie.id, updatedMovie)
        .then(() => {
          this.closeEditMovieModal();
          this.fetchMovies(); // Re-fetch movies to update list
        })
        .catch(error => this.handleError(error));
    },

    addNewMovie(movie) {
      MovieService.addNewMovie(movie)
        .then(newMovie => {
          this.movies.push(newMovie);
          this.sortMovies('title');
        })
        .catch(error => this.handleError(error));
    },
    
    showAddMovieModal() {
      if (!this.error) { // Sprawdź czy nie ma błędu przed pokazaniem modalu
        this.editingMovie = { title: '', director: '', year: null };
       this.isEditMovieModalVisible = true;
      }
    },

    closeEditMovieModal() {
      this.isEditMovieModalVisible = false;
    },

    editMovie(movie) {
      this.editingMovie = { ...movie };
      this.isEditMovieModalVisible = true;
    },

    deleteMovie(id) {
      if (!confirm('Czy na pewno chcesz usunąć ten film?')) return;

      MovieService.deleteMovie(id)
        .then(() => {
          this.movies = this.movies.filter(m => m.id !== id);
        })
        .catch(error => this.handleError(error));
    },

    sortMovies(key) {
      if (key === 'title') {
        this.movies.sort((a, b) => a[key].localeCompare(b[key], 'pl', { sensitivity: 'base' }));
      } else if (key === 'year') {
        this.movies.sort((a, b) => a[key] - b[key]);
      }
    },

    handleError(error) {
      this.error = error;
      this.isEditMovieModalVisible = false;
      this.isExternalMoviesModalVisible = false;
      console.error('Error while fetching movies:', error.message);
    }
  }
};
</script>
