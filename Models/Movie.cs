using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_jlcrawford3.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        [Required]
        [Url]
        [RegularExpression(@"https:\/\/www\.imdb\.com\/title\/tt\d{7,8}.*", ErrorMessage = "Please enter a valid HTTPS IMDb Movie URL.")]
        public string Imdb { get; set; } = default!;

        [Required]
        public string Genre { get; set; } = default!;

        [Required]
        [Range(1888, 2024)]
        public int Year { get; set; }

        [Required]
        [Url]
        [RegularExpression(@"(https:)(.*)*\.(?:jpg|jpeg|gif|png|webp)", ErrorMessage = "Please enter a valid HTTPS image URL.")]
        public string Poster { get; set; } = default!;

        // Navigation property for the many-to-many relationship between Movies and Actors
        public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
