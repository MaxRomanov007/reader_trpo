using System.Linq;
using App.Domain.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BookService;

namespace App.Pages.Admin;

public partial class OrdersPage : UserControl
{
    public OrdersPage()
    {
        InitializeComponent();

        UpdateSource();
    }

    private async void DoneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: ModifiedOrder order })
        {
            return; 
        }

        await Orders.SetOrderDone(order.Order);
        UpdateSource();
    }

    private void UpdateSource()
    {
        var orders = Orders.GetIncompleteOrders();

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