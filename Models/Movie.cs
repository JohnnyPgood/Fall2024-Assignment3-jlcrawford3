using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_jlcrawford3.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        [Url]
        public required string Imdb { get; set; }

        [Required]
        public required string Genre { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [Url]
        public required string Poster { get; set; }

        public required ICollection<MovieActor> MovieActors { get; set; }
    }
}
