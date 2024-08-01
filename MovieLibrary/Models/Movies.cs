#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MovieLibrary.Models
{
    public partial class Movies
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public double? Imdbrating { get; set; }

        public string ImageUrl { get; set; }

        public bool? Watched { get; set; }

        [DataType(DataType.Date)]
        public DateTime? WatchedDate { get; set; }

        [Required(ErrorMessage = "Release Year is required")]
        public int? ReleaseYear { get; set; }
   
        public virtual Categories Category { get; set; }

        public virtual ICollection<Actors> Actor { get; set; } = new List<Actors>();

        public virtual ICollection<Availabilities> Availability { get; set; } = new List<Availabilities>();

        [NotMapped]
        public IFormFile ImageFile { get; set; }


    }
}
