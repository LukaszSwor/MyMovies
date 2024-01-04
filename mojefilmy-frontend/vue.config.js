module.exports = {
  devServer: {
    proxy: {
      '^/MyMovies': {
        target: 'https://filmy.programdemo.pl',
        changeOrigin: true,
        logLevel: 'debug', 
        ws: true,
      },
    },
  },
};
