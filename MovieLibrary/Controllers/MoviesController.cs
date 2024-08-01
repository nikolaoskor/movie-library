using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MovieLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MovieLibrary.Controllers
{
    public class MoviesController : Controller
    {
        private readonly moviesLibraryDB _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MoviesController(moviesLibraryDB context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int[] categoryIds, string watched, int? actorId, bool? viewAll, double? minRating, double? maxRating, int? minYear, int? maxYear, int[] availabilityIds, string searchTerm)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "CategoryId", "CategoryName");

            var actors = await _context.Actors.ToListAsync();
            ViewData["Actors"] = new SelectList(actors, "ActorId", "Name");

            var availabilities = await _context.Availabilities.ToListAsync();
            ViewData["Availabilities"] = new SelectList(availabilities, "AvailabilityId", "AvailabilityName");

            var movies = _context.Movies.Include(m => m.Category).Include(m => m.Actor).Include(m => m.Availability).AsQueryable();

            // Apply filters based on user input
            if (categoryIds != null && categoryIds.Length > 0)
            {
                movies = movies.Where(m => categoryIds.Contains(m.CategoryId));
            }

            if (!string.IsNullOrEmpty(watched))
            {
                if (watched == "YES")
                {
                    movies = movies.Where(m => m.Watched == true);
                }
                else if (watched == "NO")
                {
                    movies = movies.Where(m => m.Watched == false);
                }
            }


            if (actorId.HasValue)
            {
                movies = movies.Where(m => m.Actor.Any(a => a.ActorId == actorId.Value));
            }

            if (availabilityIds != null && availabilityIds.Length > 0)
            {
                movies = movies.Where(m => m.Availability.Any(a => availabilityIds.Contains(a.AvailabilityId)));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                movies = movies.Where(m => m.Title.Contains(searchTerm));
            }

            if (viewAll.HasValue && viewAll == true)
            {
            }

            var minMovieYear = await _context.Movies.MinAsync(m => m.ReleaseYear);
            var maxMovieYear = await _context.Movies.MaxAsync(m => m.ReleaseYear);

            minYear ??= minMovieYear;
            maxYear ??= maxMovieYear;

            movies = movies.Where(m => m.ReleaseYear >= minYear && m.ReleaseYear <= maxYear);

            var minMovieRating = await _context.Movies.Where(m => m.Imdbrating.HasValue).MinAsync(m => m.Imdbrating);
            var maxMovieRating = await _context.Movies.Where(m => m.Imdbrating.HasValue).MaxAsync(m => m.Imdbrating);

            minRating ??= minMovieRating;
            maxRating ??= maxMovieRating;

            movies = movies.Where(m => (m.Imdbrating >= minRating && m.Imdbrating <= maxRating) || !m.Imdbrating.HasValue);

            ViewData["MinYear"] = minMovieYear;
            ViewData["MaxYear"] = maxMovieYear;
            ViewData["MinRating"] = minMovieRating;
            ViewData["MaxRating"] = maxMovieRating;

            ViewData["SearchTerm"] = searchTerm;

            return View(await movies.ToListAsync());
        }

        // Details
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Actor)
                .Include(m => m.Availability)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }

        // Create
        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["Actors"] = new SelectList(_context.Actors, "ActorId", "Name");
            ViewData["Availabilities"] = new SelectList(_context.Availabilities, "AvailabilityId", "AvailabilityName"); // Προσθέτουμε τις διαθεσιμότητες
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Title,CategoryId,Imdbrating,ImageUrl,ImageFile,Watched,WatchedDate,ReleaseYear")] Movies movies, int[] actorIds, int[] availabilityIds)
        {
            if (ModelState.IsValid)
            {
                // Save the image to the icon folder
                if (movies.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileName(movies.ImageFile.FileName);
                    string iconFolderPath = Path.Combine(wwwRootPath, "icon");
                    string path = Path.Combine(iconFolderPath, fileName);

                    if (!Directory.Exists(iconFolderPath))
                    {
                        Directory.CreateDirectory(iconFolderPath);
                    }

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await movies.ImageFile.CopyToAsync(fileStream);
                    }

                    movies.ImageUrl = $"/icon/{fileName}";
                }
                else
                {
                    // Set default image if no image is uploaded
                    movies.ImageUrl = "/icon/film-movie.jpeg";
                }

                // Add selected actors to the movie
                if (actorIds != null)
                {
                    foreach (var actorId in actorIds)
                    {
                        var actor = await _context.Actors.FindAsync(actorId);
                        if (actor != null)
                        {
                            movies.Actor.Add(actor);
                        }
                    }
                }

                // Add selected availabilities to the movie
                if (availabilityIds != null)
                {
                    foreach (var availabilityId in availabilityIds)
                    {
                        var availability = await _context.Availabilities.FindAsync(availabilityId);
                        if (availability != null)
                        {
                            movies.Availability.Add(availability);
                        }
                    }
                }

                _context.Add(movies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", movies.CategoryId);
            ViewData["Actors"] = new SelectList(_context.Actors, "ActorId", "Name");
            ViewData["Availabilities"] = new SelectList(_context.Availabilities, "AvailabilityId", "AvailabilityName"); // Προσθήκη των διαθεσιμοτήτων στο ViewData
            return View(movies);
        }

        // Edit 
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Actor)
                .Include(m => m.Availability) // Include availabilities
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", movie.CategoryId);

            var allActors = await _context.Actors.ToListAsync();
            var selectedActors = new HashSet<int>(movie.Actor.Select(a => a.ActorId));
            var actorList = allActors.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.ActorId.ToString(),
                Selected = selectedActors.Contains(a.ActorId)
            }).ToList();
            ViewData["Actors"] = actorList;

            var allAvailabilities = await _context.Availabilities.ToListAsync();
            var selectedAvailabilities = new HashSet<int>(movie.Availability.Select(a => a.AvailabilityId));
            var availabilityList = allAvailabilities.Select(a => new SelectListItem
            {
                Text = a.AvailabilityName,
                Value = a.AvailabilityId.ToString(),
                Selected = selectedAvailabilities.Contains(a.AvailabilityId)
            }).ToList();
            ViewData["Availabilities"] = availabilityList;

            ViewData["WatchedOptions"] = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Text = "True", Value = "true" },
        new SelectListItem { Text = "False", Value = "false" }
    }, "Value", "Text", movie.Watched.ToString().ToLower());

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,CategoryId,Imdbrating,ImageUrl,Watched,WatchedDate,ReleaseYear,ImageFile")] Movies movie, int[] actorIds, int[] availabilityIds)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (movie.ImageFile != null)
                    {
                        var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/icon");
                        if (!Directory.Exists(imagesPath))
                        {
                            Directory.CreateDirectory(imagesPath);
                        }

                        var fileName = Path.GetFileName(movie.ImageFile.FileName);
                        var filePath = Path.Combine(imagesPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await movie.ImageFile.CopyToAsync(fileStream);
                        }

                        movie.ImageUrl = Path.Combine("/icon", fileName);
                    }
                    else
                    {
                        movie.ImageUrl = _context.Movies.AsNoTracking().FirstOrDefault(f => f.MovieId == id).ImageUrl; // Keep existing ImageUrl
                    }


                    // Update actors
                    var existingMovie = await _context.Movies
                        .Include(m => m.Actor)
                        .Include(m => m.Availability) 
                        .FirstOrDefaultAsync(m => m.MovieId == id);

                    if (existingMovie == null)
                    {
                        return NotFound();
                    }

                    existingMovie.Title = movie.Title;
                    existingMovie.CategoryId = movie.CategoryId;
                    existingMovie.Imdbrating = movie.Imdbrating;
                    existingMovie.ImageUrl = movie.ImageUrl;
                    existingMovie.Watched = movie.Watched;

                    // Automatically set WatchedDate based on Watched value
                    if (movie.Watched == true)
                    {
                        existingMovie.WatchedDate = DateTime.Now;
                    }
                    else
                    {
                        existingMovie.WatchedDate = null;
                    }

                    existingMovie.ReleaseYear = movie.ReleaseYear;

                    existingMovie.Actor.Clear();
                    if (actorIds != null)
                    {
                        foreach (var actorId in actorIds)
                        {
                            var actor = await _context.Actors.FindAsync(actorId);
                            if (actor != null)
                            {
                                existingMovie.Actor.Add(actor);
                            }
                        }
                    }

                    existingMovie.Availability.Clear();
                    if (availabilityIds != null)
                    {
                        foreach (var availabilityId in availabilityIds)
                        {
                            var availability = await _context.Availabilities.FindAsync(availabilityId);
                            if (availability != null)
                            {
                                existingMovie.Availability.Add(availability);
                            }
                        }
                    }

                    _context.Update(existingMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesExists(movie.MovieId))
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

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", movie.CategoryId);
            ViewData["WatchedOptions"] = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Text = "True", Value = "true" },
        new SelectListItem { Text = "False", Value = "false" }
    }, "Value", "Text", movie.Watched.ToString().ToLower());

            return View(movie);
        }

        // Delete
        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _context.Movies
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Actor) 
                .Include(m => m.Availability)
                .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Actor.Clear();

            movie.Availability.Clear();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private bool MoviesExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}