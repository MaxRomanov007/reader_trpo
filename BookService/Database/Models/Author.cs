using System;
using System.Collections.Generic;

namespace BookService.Database.Models;

public partial class Author
{
    public long Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    
    public override bool Equals(object? obj)
    {
        if (obj is Author other && other.Id != 0)
        {
            return Id == other.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
