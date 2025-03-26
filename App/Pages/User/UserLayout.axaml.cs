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

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new AuthorizationPage());
        Session.UserId = 0;
    }
}