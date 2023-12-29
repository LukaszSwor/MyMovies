module.exports = {
  devServer: {
    proxy: {
      '/MyMovies': {
        target: 'https://filmy.programdemo.pl',
        changeOrigin: true,
        pathRewrite: { '^/MyMovies': '/MyMovies' },
      },
    },
  },
};
