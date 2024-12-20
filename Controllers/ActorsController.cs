using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_jlcrawford3.Data;
using Fall2024_Assignment3_jlcrawford3.Models;
using Fall2024_Assignment3_jlcrawford3.Models.ViewModels;
using Fall2024_Assignment3_jlcrawford3.Services;

namespace Fall2024_Assignment3_jlcrawford3.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AIService _aiService;

        public ActorsController(ApplicationDbContext context, AIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load actor details with related movies
            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                    .ThenInclude(ma => ma.Movie)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            // Create and load the view model
            var viewModel = new ActorDetailsViewModel
            {
                Actor = actor,
                Movies = actor.MovieActors?.Select(ma => ma.Movie!).ToList() ?? new List<Movie>()
            };

            return View(viewModel);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,Age,Imdb,Photo")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                actor.Imdb = SanitizeImdbUrl(actor.Imdb);
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,Age,Imdb,Photo")] Actor actor)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    actor.Imdb = SanitizeImdbUrl(actor.Imdb);
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(a => a.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }

        private string SanitizeImdbUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            var match = System.Text.RegularExpressions.Regex.Match(url, @"^(https:\/\/www\.imdb\.com\/name\/nm\d+)");
            if (match.Success)
            {
                return match.Value+"/";
            }
            return url;
        }

        [HttpGet]
        public async Task<IActionResult> GetActorTweets(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            try
            {
                var tweets = await _aiService.GenerateActorTweetsAsync(
                    actor.Name,
                    actor.Gender,
                    actor.Age
                );
                var overallSentiment = tweets.Average(t => t.Sentiment);
                
                var formattedTweets = tweets.Select(t => new
                {
                    tweet = t.Tweet,
                    sentiment = t.Sentiment
                });

                return Json(new { 
                    tweets = formattedTweets,
                    overallSentiment = overallSentiment
                });
            }
            catch (AIService.AIServiceException)
            {
                return Json(new
                {
                    tweets = new[] { new { tweet = "Oops! Something went wrong... Tweets are temporarily unavailable.", sentiment = 0.0 } },
                    overallSentiment = 0.0
                });
            }
        }
    }
}
