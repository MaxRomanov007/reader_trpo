using System.Collections.Generic;
using BookService.Database.Models;

namespace App.Domain.Models;

public class ModifiedOrder
{
    public List<ModifiedBook> Books { get; set; }
    public Order Order { get; set; }
}