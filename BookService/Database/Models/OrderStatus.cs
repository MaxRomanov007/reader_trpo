namespace BookService.Database.Models;

public partial class OrderStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public const string InProgress = "in_progress";
    public const string Completed = "completed";
    public const string Basket = "basket";
}