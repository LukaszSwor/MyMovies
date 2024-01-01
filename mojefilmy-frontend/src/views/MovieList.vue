<!-- MovieList.vue -->
<template>
  <div class="movie-list">
    <table class="table">
      <thead>
        <tr>
          <th>Tytuł</th>
          <th>Reżyser</th>
          <th>Rok</th>
          <th>Akcje</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="movie in movies" :key="movie.id">
          <td>{{ movie.title }}</td>
          <td>{{ movie.director }}</td>
          <td>{{ movie.year }}</td>
          <td>
            <button class="btn btn-primary" @click="editMovie(movie)">Edytuj</button>
            <button class="btn btn-danger" @click="deleteMovie(movie.id)">Usuń</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: 'MovieList',
  props: {
    movies: Array
  },
  methods: {
    editMovie(movie) {
      // Emituje zdarzenie 'edit-movie' z danymi filmu do rodzica
      this.$emit('edit-movie', movie);
    },
    deleteMovie(id) {
      // Emituje zdarzenie 'delete-movie' z id filmu do rodzica
      this.$emit('delete-movie', id);
    }
  }
};
</script>

<style>
/* Styl dla MovieList, jeśli jest potrzebny */
</style>