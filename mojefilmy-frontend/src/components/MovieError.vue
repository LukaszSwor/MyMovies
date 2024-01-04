<template>
  <div v-if="error" class="alert alert-danger">
    {{ error.message }}
  </div>
</template>

<script>
export default {
  props: ['error']
};
</script>
