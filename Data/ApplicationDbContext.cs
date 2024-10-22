using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_jlcrawford3.Models;

namespace Fall2024_Assignment3_jlcrawford3.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<Actor> Actors { get; set; } = default!;
    public DbSet<MovieActor> MovieActors { get; set; } = default!;
}
