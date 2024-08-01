#nullable disable
using System;
using System.Collections.Generic;

namespace MovieLibrary.Models;

public partial class Availabilities
{
    public int AvailabilityId { get; set; }

    public string AvailabilityName { get; set; }

    public virtual ICollection<Movies> Movie { get; set; } = new List<Movies>();
}