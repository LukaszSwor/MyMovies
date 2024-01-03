// vue.config.js
module.exports = {
  devServer: {
    proxy: {
      '^/MyMovies': {
        target: 'https://filmy.programdemo.pl',
        changeOrigin: true,
        logLevel: 'debug', // Dodaj to dla lepszego debugowania
        ws: true,
      },
    },
  },
};
