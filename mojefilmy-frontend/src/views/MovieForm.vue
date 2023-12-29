<!-- MovieForm.vue -->
<template>
  <div class="movie-form">
    <form @submit.prevent="submitForm">
      <div class="mb-3">
        <label for="title" class="form-label">Tytuł</label>
        <input type="text" class="form-control" id="title" v-model="movie.title" required>
      </div>
      <div class="mb-3">
        <label for="director" class="form-label">Reżyser</label>
        <input type="text" class="form-control" id="director" v-model="movie.director" required>
      </div>
      <div class="mb-3">
        <label for="year" class="form-label">Rok</label>
        <input type="number" class="form-control" id="year" v-model.number="movie.year" required>
      </div>
      <button type="submit" class="btn btn-success">{{ movie.id ? 'Aktualizuj' : 'Dodaj' }}</button>
    </form>
  </div>
</template>

<script>
export default {
  name: 'MovieForm',
  props: {
    movieData: {
      type: Object,
      default: () => ({ title: '', director: '', year: null })
    }
  },
  data() {
    return {
      movie: { ...this.movieData }
    };
  },
  methods: {
    submitForm() {
      // Emituje zdarzenie z obiektem filmu do zapisania
      this.$emit('save-movie', this.movie);
      this.resetForm();
    },
    resetForm() {
      // Resetuje formularz do wartości początkowych
      this.movie = { title: '', director: '', year: null };
    }
  },
  watch: {
    movieData: {
      deep: true,
      handler(newVal) {
        // Reaguje na zmiany w prop movieData i ustawia lokalny stan movie
        this.movie = { ...newVal };
      }
    }
  }
};
</script>

