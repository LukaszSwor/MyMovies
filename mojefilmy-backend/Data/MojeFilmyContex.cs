using Microsoft.EntityFrameworkCore;
using mojefilmy_backend.Models;

namespace mojefilmy_backend.Data
{
    public class MojeFilmyContext : DbContext
    {
        public MojeFilmyContext(DbContextOptions<MojeFilmyContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}