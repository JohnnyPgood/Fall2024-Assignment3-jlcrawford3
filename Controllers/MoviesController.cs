using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_jlcrawford3.Data;
using Fall2024_Assignment3_jlcrawford3.Models;
using Fall2024_Assignment3_jlcrawford3.Models.ViewModels;
using Fall2024_Assignment3_jlcrawford3.Services;

namespace Fall2024_Assignment3_jlcrawford3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AIService _aiService;

        public MoviesController(ApplicationDbContext context, AIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load movie details with related actors
            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            // Create and load the view model
             var viewModel = new MovieDetailsViewModel
            {
                Movie = movie,
                Actors = movie.MovieActors?.Select(ma => ma.Actor!).ToList() ?? new List<Actor>()
            };

            return View(viewModel);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Imdb,Genre,Year,Poster")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Imdb = SanitizeImdbUrl(movie.Imdb);
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Imdb,Genre,Year,Poster")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movie.Imdb = SanitizeImdbUrl(movie.Imdb);
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        private string SanitizeImdbUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            var match = System.Text.RegularExpressions.Regex.Match(url, @"^(https:\/\/www\.imdb\.com\/title\/tt\d+)");
            if (match.Success)
            {
                return match.Value+"/";
            }
            return url;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieReviews(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            try
            {
                var reviews = await _aiService.GenerateMovieReviewsAsync(
                    movie.Title,
                    movie.Year,
                    movie.Genre
                );
                
                var formattedReviews = reviews.Select(r => new
                {
                    review = r.Review,
                    sentiment = r.Sentiment
                });

                return Json(new { 
                    reviews = formattedReviews,
                    overallSentiment = reviews.Average(r => r.Sentiment)
                });
            }
            catch (AIService.AIServiceException)
            {
                return Json(new
                {
                    reviews = new[] { new { review = "Oops! Something went wrong... Reviews are temporarily unavailable.", sentiment = 0.0 } },
                    overallSentiment = 0.0
                });
            }
        }
    }
}
