using System.Linq;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using BookService;

namespace App.Pages.User;

public partial class UserOrdersPage : UserControl
{
    public UserOrdersPage()
    {
        InitializeComponent();

        var orders = Orders.GetUserOrders(Session.UserId);

        OrdersDataGrid.ItemsSource = orders.Select(o => new ModifiedOrder
        {
            Order = o,
            Books = o.OrderBooks.Select(ob => new ModifiedBook
            {
                Standard = ob.Book,
                InOrderCount = ob.Count
            }).ToList()
        });
    }
}