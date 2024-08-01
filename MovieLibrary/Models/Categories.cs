#nullable disable
using System;
using System.Collections.Generic;

namespace MovieLibrary.Models;

public partial class Categories
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<Movies> Movies { get; set; } = new List<Movies>();
}