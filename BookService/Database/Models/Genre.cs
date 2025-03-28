using System;
using System.Collections.Generic;

namespace BookService.Database.Models;

public partial class Genre
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
