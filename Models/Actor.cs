using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_jlcrawford3.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Gender { get; set; } = default!;

        [Required]
        [Range(0, 150)]
        public int Age { get; set; }

        [Required]
        [Url]
        [RegularExpression(@"https:\/\/www\.imdb\.com\/name\/nm\d{7,8}.*", ErrorMessage = "Please enter a valid HTTPS IMDb Actor URL.")]
        public string Imdb { get; set; } = default!;

        [Required]
        [Url]
        [RegularExpression(@"(https:)(.*)*\.(?:jpg|jpeg|gif|png|webp)", ErrorMessage = "Please enter a valid HTTPS image URL.")]
        public string Photo { get; set; } = default!;

        // Navigation property for the many-to-many relationship between Actors and Movies
        public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}