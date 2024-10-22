using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_jlcrawford3.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [Url]
        public required string Imdb { get; set; }

        [Required]
        [Url]
        public required string Photo { get; set; }

        public required ICollection<MovieActor> MovieActors { get; set; }
    }
}