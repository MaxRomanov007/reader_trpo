using System;
using System.Collections.Generic;

namespace BookService.Database.Models;

public partial class OrderBook
{
    public long Id { get; set; }

    public long BookId { get; set; }

    public long OrderId { get; set; }

    public int Count { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
