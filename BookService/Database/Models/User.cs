using System;
using System.Collections.Generic;

namespace BookService.Database.Models;

public partial class User
{
    public long Id { get; set; }

    public long RoleId { get; set; }

    public string Email { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual UserRole Role { get; set; } = null!;
}
