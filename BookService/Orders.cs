using System.Collections.Immutable;
using BookService.Database;
using BookService.Database.models;
using BookService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookService;

public static class Orders
{
    public static int GetCountBooksInUserBasket(long userId, long bookId)
    {
        using var context = new BooksContext();

        var orderBook = context.OrderBooks
            .Include(o => o.Order)
            .Include(o => o.Order.Status)
            .FirstOrDefault(o =>
                o.Order.Status.Name == OrderStatus.Basket &&
                o.Order.UserId == userId &&
                o.BookId == bookId);

        return (orderBook ?? new OrderBook()).Count;
    }

    public static async Task CreateOrder(long userId, long bookId)
    {
        await using var context = new BooksContext();

        var order = await context.Orders
            .Include(o => o.Status)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status.Name == OrderStatus.Basket);
        if (order is null)
        {
            order = new Order
            {
                Date = DateTime.Today,
                Status = context.OrderStatuses.FirstOrDefault(o => o.Name == OrderStatus.Basket) ?? new OrderStatus(),
                UserId = userId
            };

            context.Orders.Add(order);

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                // ignored
            }
        }

        var orderBook = new OrderBook
        {
            BookId = bookId,
            Count = 1,
            OrderId = order.Id
        };

        context.OrderBooks.Add(orderBook);

        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            // ignored
        }
    }

    public static async Task ChangeCount(long userId, long bookId, int count)
    {
        await using var context = new BooksContext();

        var orderBook = await context.OrderBooks
            .Include(o => o.Order)
            .Include(o => o.Order.Status)
            .FirstOrDefaultAsync(
                o => o.Order.UserId == userId &&
                     o.Order.Status.Name == OrderStatus.Basket &&
                     o.BookId == bookId);

        if (orderBook is null)
        {
            return;
        }

        if (count == 0)
        {
            context.OrderBooks.Remove(orderBook);
        }
        else
        {
            orderBook.Count = count;
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            // ignored
        }

        var order = await context.Orders
            .Include(o => o.OrderBooks)
            .Include(o => o.Status)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status.Name == OrderStatus.Basket);

        if (order is null || order.OrderBooks.Count > 0)
        {
            return;
        }

        context.Orders.Remove(order);

        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            // ignored
        }
    }

    public static List<OrderBook>? GetUserBooksInOrder(long userId)
    {
        using var context = new BooksContext();
        
        var order = context.Orders
            .Include(o => o.Status)
            .Include(o => o.OrderBooks).ThenInclude(orderBook => orderBook.Book)
            .FirstOrDefault(o => o.UserId == userId && o.Status.Name == OrderStatus.Basket);

        var orderBooks = order?.OrderBooks.ToList();
            
        orderBooks?.ForEach(o => o.Book.Image = Images.FullName(o.Book.Image));

        return order?.OrderBooks.ToList();
    }

    public static async Task SendOrder(long userId)
    {
        await using var context = new BooksContext();
        
        var order = context.Orders
            .Include(o => o.Status)
            .FirstOrDefault(o => o.UserId == userId && o.Status.Name == OrderStatus.Basket);

        var inProgressStatus = context.OrderStatuses.FirstOrDefault(s => s.Name == OrderStatus.InProgress);

        if (order is not null && inProgressStatus is not null)
        {
            order.Status = inProgressStatus;
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            // ignored
        }
    }

}