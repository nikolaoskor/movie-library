using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Models;
using MovieLibrary.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly moviesLibraryDB _context;

        public HomeController(ILogger<HomeController> logger, moviesLibraryDB context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get top 8 unwatched movies with highest IMDb rating
            var recommendedMovies = await _context.Movies
                .Where(m => m.Watched == false)
                .OrderByDescending(m => m.Imdbrating)
                .Take(8)
                .Include(m => m.Category)
                .ToListAsync();

            // Get latest 4 movies
            var newReleases = await _context.Movies
                .OrderByDescending(m => m.ReleaseYear)
                .Take(4)
                .Include(m => m.Category)
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                RecommendedMovies = recommendedMovies,
                NewReleases = newReleases
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
