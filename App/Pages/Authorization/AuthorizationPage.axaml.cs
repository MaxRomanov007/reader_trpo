using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using App.Pages.Admin;
using App.Pages.User;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;
using BookService.Database.Models;

namespace App.Pages.Authorization;

public partial class AuthorizationPage : UserControl
{
    private readonly Credentials _credentials = new();

    public AuthorizationPage()
    {
        InitializeComponent();
        Session.UserId = 0;
        DataContext = _credentials;
    }

    public AuthorizationPage(string message)
    {
        InitializeComponent();
        DataContext = _credentials;
        MessageTextBlock.ShowTemporaryText(message);
    }

    private async void AuthorizationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_credentials.Email == null || _credentials.Password == null)
        {
            ErrorTextBlock.ShowTemporaryText("Введите email и пароль");
            return;
        }

        EnterButton.IsEnabled = false;
        var user = await Users.Authorize(_credentials.Email, _credentials.Password);
        if (user is null)
        {
            ErrorTextBlock.ShowTemporaryText("Неверный логин или пароль");
            return;
        }

        switch (user.Role.Name)
        {
            case UserRole.User:
                Session.UserId = user.Id;
                MainContent.NavigateTo(new UserLayout());
                break;
            case UserRole.Admin:
                MainContent.NavigateTo(new AdminLayout());
                break;
        }

        EnterButton.IsEnabled = true;
    }

    private void ToRegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new RegistrationPage());
    }

    private void LostPasswordButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new LostPasswordPage());
    }
}