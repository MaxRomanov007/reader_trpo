namespace BookService.Database.Models;

public partial class Order
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long StatusId { get; set; }

    public DateTime Date { get; set; }

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}