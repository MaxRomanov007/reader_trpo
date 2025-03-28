using App.Domain.Static;
using App.Pages.Authorization;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace App.Pages.User;

public partial class UserLayout : UserControl
{
    public UserLayout()
    {
        InitializeComponent();

        UserContent.Content = UserContentControl;
        UserContentControl.Content = new BooksPage();
    }

    private void ExitButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new AuthorizationPage());
    }

    private void BasketButton_OnClick(object? sender, RoutedEventArgs e)
    {
        UserContent.NavigateTo(new BasketPage());
    }

    private void MainButton_OnClick(object? sender, RoutedEventArgs e)
    {
        UserContent.NavigateTo(new BooksPage());
    }

    private void MyOrdersButton_OnClick(object? sender, RoutedEventArgs e)
    {
        UserContent.NavigateTo(new UserOrdersPage());
    }
}