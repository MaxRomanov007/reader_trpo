using System;
using System.Collections.Generic;

namespace BookService.Database.Models;

public partial class Book
{
    public long Id { get; set; }

    public long GenreId { get; set; }

    public long AuthorId { get; set; }

    public long StatusId { get; set; }

    public decimal Cost { get; set; }

    public short Year { get; set; }

    public string Name { get; set; } = null!;

    public int Count { get; set; }

    public string Image { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();

    public virtual BookStatus Status { get; set; } = null!;
}
