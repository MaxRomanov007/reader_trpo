using System;
using System.Collections.Generic;

namespace BookService.Database.Models;

public partial class BookStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
