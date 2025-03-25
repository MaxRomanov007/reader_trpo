using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;

namespace App.Pages.Authorization;

public partial class AuthorizationPage : UserControl
{
    private readonly Credentials _credentials = new();
    public AuthorizationPage()
    {
        InitializeComponent();
        DataContext = _credentials;
    }

    public AuthorizationPage(string message)
    {
        InitializeComponent();
        DataContext = _credentials;
        TextBlockExtensions.ShowTemporaryText(MessageTextBlock, message);
    }

    private async void AuthorizationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_credentials.Email == null || _credentials.Password == null)
        {
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, "Введите email и пароль");
            return;
        }

        var result = await Users.Authorize(_credentials.Email, _credentials.Password);
        TextBlockExtensions.ShowTemporaryText(MessageTextBlock, result);
    }

    private void ToRegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new RegistrationPage());
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new LostPasswordPage());
    }
}