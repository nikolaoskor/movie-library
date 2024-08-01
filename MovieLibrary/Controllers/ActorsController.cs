using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Models;

namespace MovieLibrary.Controllers
{
    public class ActorsController : Controller
    {
        private readonly moviesLibraryDB _context;

        public ActorsController(moviesLibraryDB context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index(string searchString)
        {
            var actors = from a in _context.Actors
                         select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                actors = actors.Where(a => a.Name.Contains(searchString));
            }

            return View(await actors.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .Include(a => a.Movie) // Include the related movies
                .FirstOrDefaultAsync(m => m.ActorId == id);

            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }


        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,Name")] Actors actors)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Έλεγχος για το αν το πεδίο Name δεν είναι κενό ή null
                    if (string.IsNullOrWhiteSpace(actors.Name))
                    {
                        ModelState.AddModelError("Name", "Please give a name");
                        return View(actors);
                    }

                    _context.Add(actors);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(actors);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occurred while saving your data. Please try again.");
                return View(actors);
            }
        }


        // POST: Actors/RemoveMovie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMovie(int actorId, int movieId)
        {
            var actor = await _context.Actors
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(a => a.ActorId == actorId);

            if (actor == null)
            {
                return NotFound();
            }

            var movie = actor.Movie.FirstOrDefault(m => m.MovieId == movieId);
            if (movie != null)
            {
                actor.Movie.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id = actorId });
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .Include(a => a.Movie) // Include the related movies
                .FirstOrDefaultAsync(a => a.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,Name")] Actors actor)
        {
            if (id != actor.ActorId)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(actor.Name))
            {
                // Find the existing actor to get the current name
                var existingActor = await _context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.ActorId == id);
                if (existingActor != null)
                {
                    actor.Name = existingActor.Name;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorsExists(actor.ActorId))
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

            var actors = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actors == null)
            {
                return NotFound();
            }

            return View(actors);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(a => a.ActorId == id);

            if (actor != null)
            {
                var relatedMovies = _context.Set<Dictionary<string, object>>("MovieActors")
                    .Where(ma => (int)ma["ActorId"] == id);

                _context.RemoveRange(relatedMovies);

                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActorsExists(int id)
        {
            return _context.Actors.Any(e => e.ActorId == id);
        }
    }
}
