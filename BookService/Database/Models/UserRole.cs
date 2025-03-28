namespace BookService.Database.Models;

public partial class UserRole
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public const string Admin = "admin";
    public const string User = "user";
}