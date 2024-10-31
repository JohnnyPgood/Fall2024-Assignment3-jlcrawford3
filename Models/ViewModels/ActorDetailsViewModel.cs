using Fall2024_Assignment3_jlcrawford3.Models;

namespace Fall2024_Assignment3_jlcrawford3.Models.ViewModels;

public class ActorDetailsViewModel
{
    public Actor Actor { get; set; } = default!;
    public List<Movie> Movies { get; set; } = new();
}
