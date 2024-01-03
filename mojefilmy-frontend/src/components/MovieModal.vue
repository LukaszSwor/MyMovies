<template>
  <div class="modal-overlay" v-if="isVisible" @click.self="closeModal">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <!-- Dynamiczny tytuł dla różnych przypadków użycia modalu -->
          <h5 class="modal-title">
            <slot name="header">Dodaj filmy</slot>
          </h5>
          <button type="button" class="btn-close" @click="closeModal"></button>
        </div>
        <div class="modal-body">
          <!-- Slot na treść główną modalu -->
          <slot>
            <ul>
              <li v-for="movie in movies" :key="movie.id" class="modal-movie-item">
  <span @click="addMovie(movie)" style="cursor: pointer;">
    {{ movie.title }} ({{ movie.year }}) - Reżyseria: {{ movie.director }}
  </span>
</li>
            </ul>
          </slot>
        </div>
        <div class="modal-footer">
          <!-- Slot na stopkę modalu -->
          <slot name="footer">
            <button type="button" class="btn btn-secondary" @click="closeModal">Zamknij</button>
          </slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    movies: {
      type: Array,
      default: () => []
    },
    isVisible: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    closeModal() {
      this.$emit('close');
    },
    addMovie(movie) {
      // Emitowanie zdarzenia z wybranym filmem do dodania
      this.$emit('add-movie', movie);
    }
  }
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-dialog {
  background: white;
  border-radius: 5px;
  padding: 20px;
}

.modal-movie-item {
  margin-bottom: 10px;
    justify-content: space-between;
  align-items: center;
}
</style>
