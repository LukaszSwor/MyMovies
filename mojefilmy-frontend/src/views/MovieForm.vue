<template>
  <div class="movie-form">
    <form @submit.prevent="submitForm">
      <div class="mb-3">
        <label for="title" class="form-label">Tytuł</label>
        <input type="text" class="form-control" id="title" v-model="movie.title">
        <!-- Komunikat o błędzie dla tytułu -->
        <div v-if="v$.title.$error" class="alert alert-danger">
          Tytuł jest wymagany i nie może przekraczać 200 znaków.
        </div>
      </div>
      <div class="mb-3">
        <label for="director" class="form-label">Reżyser</label>
        <input type="text" class="form-control" id="director" v-model="movie.director" required>
      </div>
         <div class="mb-3">
        <label for="year" class="form-label">Rok</label>
        <input type="number" class="form-control" id="year" v-model.number="movie.year">
        <!-- Komunikat o błędzie dla roku -->
        <div v-if="v$.year.$error" class="alert alert-danger">
          Rok musi być pomiędzy 1900 a 2200.
        </div>
      </div>

      <button type="submit" class="btn btn-success">{{ movie.id ? 'Aktualizuj' : 'Dodaj' }}</button>
    </form>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { required, maxLength, between } from '@vuelidate/validators';

export default {
  name: 'MovieForm',
  props: {
    movieData: {
      type: Object,
      default: () => ({ title: '', director: '', year: null })
    }
  },
  setup(props, { emit }) { // Dodaj 'emit' jako argument funkcji setup
    const movie = ref({ ...props.movieData });

    const rules = {
      title: { required, maxLength: maxLength(200) },
      year: { required, between: between(1900, 2200) }
    };

    const v$ = useVuelidate(rules, movie);

      const submitForm = () => {
  v$.value.$validate();
  if (!v$.value.$invalid) {
    if (movie.value.id) {
      emit('update-movie', movie.value);
    } else {
      emit('save-movie', movie.value);
    }
    if (!movie.value.id) {
        resetForm();
      }
  }
};

    const resetForm = () => {
  movie.value = { title: '', director: '', year: null };
  v$.value.$reset(); // Resetowanie stanu walidacji
};

    return { movie, v$, submitForm, resetForm };
  }
};
</script>
