using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_jlcrawford3.Models;

namespace Fall2024_Assignment3_jlcrawford3.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Check if DB has been seeded
            if (context.Movies.Any())
            {
                return;
            }

            // Seed the database with movies
            context.Movies.AddRange(
                // 1
                new Movie
                {
                    Title = "Primer",
                    Imdb = "https://www.imdb.com/title/tt0390384/",
                    Genre = "Science Fiction",
                    Year = 2004,
                    Poster = "https://m.media-amazon.com/images/M/MV5BZTRmZDlmNzUtNjM2NS00MjBjLWFmNjQtMTczMWNkZTdiOGZmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 2
                new Movie
                {
                    Title = "Back to the Future",
                    Imdb = "https://www.imdb.com/title/tt0088763/",
                    Genre = "Science Fiction/Adventure",
                    Year = 1985,
                    Poster = "https://m.media-amazon.com/images/M/MV5BZmM3ZjE0NzctNjBiOC00MDZmLTgzMTUtNGVlOWFlOTNiZDJiXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 3
                new Movie
                {
                    Title = "12 Monkeys",
                    Imdb = "https://www.imdb.com/title/tt0114746/",
                    Genre = "Science Fiction/Mystery",
                    Year = 1995,
                    Poster = "https://m.media-amazon.com/images/M/MV5BMDQwNDY0M2MtOGFmNy00ZjI5LTkzN2ItMzg5M2IwZTZjY2MyXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 4
                new Movie
                {
                    Title = "Donnie Darko",
                    Imdb = "https://www.imdb.com/title/tt0246578/",
                    Genre = "Science Fiction/Drama",
                    Year = 2001,
                    Poster = "https://m.media-amazon.com/images/M/MV5BMWE3NTYzZmEtM2U5MS00MDZhLTk2ZTQtZTgzNjg0ZGQ5ZjM0XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 5
                new Movie
                {
                    Title = "Groundhog Day",
                    Imdb = "https://www.imdb.com/title/tt0107048/",
                    Genre = "Comedy/Fantasy",
                    Year = 1993,
                    Poster = "https://m.media-amazon.com/images/M/MV5BOWE3MjQ3ZDAtNDQ2MC00YjBjLTk0ZWYtNjQ0YzQ4YWE3YTEyXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 6
                new Movie
                {
                    Title = "Run Lola Run",
                    Imdb = "https://www.imdb.com/title/tt0130827/",
                    Genre = "Thriller/Drama",
                    Year = 1998,
                    Poster = "https://m.media-amazon.com/images/M/MV5BMzA0NDdiNDgtNjI4Ny00OTc0LTg3NGMtZDBlNmI1ZGUxYTg2XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 7
                new Movie
                {
                    Title = "Edge of Tomorrow",
                    Imdb = "https://www.imdb.com/title/tt1631867/",
                    Genre = "Science Fiction/Action",
                    Year = 2014,
                    Poster = "https://m.media-amazon.com/images/M/MV5BMTc5OTk4MTM3M15BMl5BanBnXkFtZTgwODcxNjg3MDE@._V1_FMjpg_UX1000_.jpg"
                },
                // 8
                new Movie
                {
                    Title = "Looper",
                    Imdb = "https://www.imdb.com/title/tt1276104/",
                    Genre = "Science Fiction/Action",
                    Year = 2012,
                    Poster = "https://m.media-amazon.com/images/M/MV5BMTg5NTA3NTg4NF5BMl5BanBnXkFtZTcwNTA0NDYzOA@@._V1_FMjpg_UX1000_.jpg"
                },
                // 9
                new Movie
                {
                    Title = "Interstellar",
                    Imdb = "https://www.imdb.com/title/tt0816692/",
                    Genre = "Science Fiction/Drama",
                    Year = 2014,
                    Poster = "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 10
                new Movie
                {
                    Title = "Terminator 2: Judgement Day",
                    Imdb = "https://www.imdb.com/title/tt0103064/",
                    Genre = "Science Fiction/Action",
                    Year = 1991,
                    Poster = "https://m.media-amazon.com/images/M/MV5BNGMyMGNkMDUtMjc2Ni00NWFlLTgyODEtZTY2MzBiZTg0OWZiXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                }
            );
            context.SaveChanges();

            // Seed the database with actors
            context.Actors.AddRange(
                // 1
                new Actor
                {
                    Name = "Shane Carruth",
                    Gender = "Male",
                    Age = 51,
                    Imdb = "https://www.imdb.com/name/nm1503403/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BNzU5MTYwNTg5NV5BMl5BanBnXkFtZTYwMzkxNTUz._V1_FMjpg_UX1000_.jpg"
                },
                // 2
                new Actor
                {
                    Name = "David Sullivan",
                    Gender = "Male",
                    Age = 47,
                    Imdb = "https://www.imdb.com/name/nm1503383/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTU5OTQ3Mjg0N15BMl5BanBnXkFtZTgwNjAwMDE1MDE@._V1_FMjpg_UX1000_.jpg"
                },
                // 3
                new Actor
                {
                    Name = "Michael J. Fox",
                    Gender = "Male",
                    Age = 62,
                    Imdb = "https://www.imdb.com/name/nm0000150/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTcwNzM0MjE4NF5BMl5BanBnXkFtZTcwMDkxMzEwMw@@._V1_FMjpg_UX1000_.jpg"
                },
                // 4
                new Actor
                {
                    Name = "Christopher Lloyd",
                    Gender = "Male",
                    Age = 84,
                    Imdb = "https://www.imdb.com/name/nm0000502/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BZDM3MjdhZjktMGEyMi00NDRmLThhNjUtNGU3MGE1ZDk3MmI0XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 5
                new Actor
                {
                    Name = "Bruce Willis",
                    Gender = "Male",
                    Age = 68,
                    Imdb = "https://www.imdb.com/name/nm0000246/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMjA0MjMzMTE5OF5BMl5BanBnXkFtZTcwMzQ2ODE3Mw@@._V1_FMjpg_UX1000_.jpg"
                },
                // 6
                new Actor
                {
                    Name = "Brad Pitt",
                    Gender = "Male",
                    Age = 59,
                    Imdb = "https://www.imdb.com/name/nm0000093/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMjA1MjE2MTQ2MV5BMl5BanBnXkFtZTcwMjE5MDY0Nw@@._V1_FMjpg_UX1000_.jpg"
                },
                // 7
                new Actor
                {
                    Name = "Jake Gyllenhaal",
                    Gender = "Male",
                    Age = 42,
                    Imdb = "https://www.imdb.com/name/nm0350453/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BNjA0MTU2NDY3MF5BMl5BanBnXkFtZTgwNDU4ODkzMzE@._V1_FMjpg_UX1000_.jpg"
                },
                // 8
                new Actor
                {
                    Name = "Jena Malone",
                    Gender = "Female",
                    Age = 38,
                    Imdb = "https://www.imdb.com/name/nm0540441/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTU0NDM5OTE0N15BMl5BanBnXkFtZTcwMzMzNjM0Nw@@._V1_FMjpg_UX1000_.jpg"
                },
                // 9
                new Actor
                {
                    Name = "Bill Murray",
                    Gender = "Male",
                    Age = 73,
                    Imdb = "https://www.imdb.com/name/nm0000195/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTQ1OTM0MjEwOF5BMl5BanBnXkFtZTYwNTQwNzI1._V1_FMjpg_UX1000_.jpg"
                },
                // 10
                new Actor
                {
                    Name = "Andie MacDowell",
                    Gender = "Female",
                    Age = 65,
                    Imdb = "https://www.imdb.com/name/nm0000510/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMGI5OGE2ZWQtNTZlMS00NjdhLTgzMTMtMzVjNTY1N2ExZTg2XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 11
                new Actor
                {
                    Name = "Franka Potente",
                    Gender = "Female",
                    Age = 49,
                    Imdb = "https://www.imdb.com/name/nm0004376/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BNDc5ODlmM2EtNTlkMC00MDVlLWJiNzctODQyNzdhY2Y3NDA3XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 12
                new Actor
                {
                    Name = "Moritz Bleibtreu",
                    Gender = "Male",
                    Age = 53,
                    Imdb = "https://www.imdb.com/name/nm0001953/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTk1Nzc5NTk4MF5BMl5BanBnXkFtZTcwOTQyMDMxNw@@._V1_FMjpg_UX1000_.jpg"
                },
                // 13
                new Actor
                {
                    Name = "Tom Cruise",
                    Gender = "Male",
                    Age = 61,
                    Imdb = "https://www.imdb.com/name/nm0000129/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMmU1YWU1NmMtMjAyMi00MjFiLWFmZmUtOTc1ZjI5ODkxYmQyXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 14
                new Actor
                {
                    Name = "Emily Blunt",
                    Gender = "Female",
                    Age = 40,
                    Imdb = "https://www.imdb.com/name/nm1289434/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTUxNDY4MTMzM15BMl5BanBnXkFtZTcwMjg5NzM2Ng@@._V1_FMjpg_UX1000_.jpg"
                },
                // 15
                new Actor
                {
                    Name = "Joseph Gordon-Levitt",
                    Gender = "Male",
                    Age = 42,
                    Imdb = "https://www.imdb.com/name/nm0330687/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTY3NTk0NDI3Ml5BMl5BanBnXkFtZTgwNDA3NjY0MjE@._V1_FMjpg_UX1000_.jpg"
                },
                // 16
                new Actor
                {
                    Name = "Matthew McConaughey",
                    Gender = "Male",
                    Age = 53,
                    Imdb = "https://www.imdb.com/name/nm0000190/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTg0MDc3ODUwOV5BMl5BanBnXkFtZTcwMTk2NjY4Nw@@._V1_FMjpg_UX1000_.jpg"
                },
                // 17
                new Actor
                {
                    Name = "Anne Hathaway",
                    Gender = "Female",
                    Age = 40,
                    Imdb = "https://www.imdb.com/name/nm0004266/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BNzA0MWI3ZDgtMDVkZS00NTVhLTkwMzQtNmNlODk5MDYzMzFmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                },
                // 18
                new Actor
                {
                    Name = "Arnold Schwarzenegger",
                    Gender = "Male",
                    Age = 76,
                    Imdb = "https://www.imdb.com/name/nm0000216/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BMTI3MDc4NzUyMV5BMl5BanBnXkFtZTcwMTQyMTc5MQ@@._V1_FMjpg_UX1000_.jpg"
                },
                // 19
                new Actor
                {
                    Name = "Linda Hamilton",
                    Gender = "Female",
                    Age = 66,
                    Imdb = "https://www.imdb.com/name/nm0000157/",
                    Photo = "https://m.media-amazon.com/images/M/MV5BZTI1MmRmYmMtYTZiMy00NGFkLThkMGEtNjgyYjFhMzE2NjUzXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
                }
            );
            context.SaveChanges();

            // Seed the database with movieactors links
            context.MovieActors.AddRange(
                // Primer
                new MovieActor { MovieId = 1, ActorId = 1 },  // Shane Carruth
                new MovieActor { MovieId = 1, ActorId = 2 },  // David Sullivan

                // Back to the Future
                new MovieActor { MovieId = 2, ActorId = 3 },  // Michael J. Fox
                new MovieActor { MovieId = 2, ActorId = 4 },  // Christopher Lloyd

                // 12 Monkeys
                new MovieActor { MovieId = 3, ActorId = 5 },  // Bruce Willis
                new MovieActor { MovieId = 3, ActorId = 6 },  // Brad Pitt

                // Donnie Darko
                new MovieActor { MovieId = 4, ActorId = 7 },  // Jake Gyllenhaal
                new MovieActor { MovieId = 4, ActorId = 8 },  // Jena Malone

                // Groundhog Day
                new MovieActor { MovieId = 5, ActorId = 9 },  // Bill Murray
                new MovieActor { MovieId = 5, ActorId = 10 }, // Andie MacDowell

                // Run Lola Run
                new MovieActor { MovieId = 6, ActorId = 11 }, // Franka Potente
                new MovieActor { MovieId = 6, ActorId = 12 }, // Moritz Bleibtreu

                // Edge of Tomorrow
                new MovieActor { MovieId = 7, ActorId = 13 }, // Tom Cruise
                new MovieActor { MovieId = 7, ActorId = 14 }, // Emily Blunt

                // Looper
                new MovieActor { MovieId = 8, ActorId = 15 }, // Joseph Gordon-Levitt
                new MovieActor { MovieId = 8, ActorId = 5 },  // Bruce Willis

                // Interstellar
                new MovieActor { MovieId = 9, ActorId = 16 }, // Matthew McConaughey
                new MovieActor { MovieId = 9, ActorId = 17 }, // Anne Hathaway

                // Terminator 2
                new MovieActor { MovieId = 10, ActorId = 18 }, // Arnold Schwarzenegger
                new MovieActor { MovieId = 10, ActorId = 19 }  // Linda Hamilton
            );
            context.SaveChanges();
        }
    }
}
