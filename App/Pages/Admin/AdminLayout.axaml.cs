using App.Domain.Static;
using App.Pages.Authorization;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace App.Pages.Admin;

public partial class AdminLayout : UserControl
{
    public AdminLayout()
    {
        InitializeComponent();

        AdminContent.Content = AdminContentControl;
        AdminContentControl.Content = new AdminBooksPage();
    }

    private void BooksButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AdminContent.NavigateTo(new AdminBooksPage());
    }

    private void ExitButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new AuthorizationPage());
    }

    private void OrdersButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AdminContent.NavigateTo(new OrdersPage());
    }

    private void CreateReportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AdminContent.NavigateTo(new CreateReportPage());
    }
}