#nullable disable
using System;
using System.Collections.Generic;

namespace MovieLibrary.Models;

public partial class Actors
{
    public int ActorId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Movies> Movie { get; set; } = new List<Movies>();
}