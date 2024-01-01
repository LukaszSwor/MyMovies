using System.ComponentModel.DataAnnotations;

namespace mojefilmy_backend.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
    }
}
