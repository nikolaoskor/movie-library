using MovieLibrary.Models;
using System.Collections.Generic;

namespace MovieLibrary.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Movies> RecommendedMovies { get; set; }
        public IEnumerable<Movies> NewReleases { get; set; }
    }
}
