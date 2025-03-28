using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;

namespace App.Pages.Authorization;

public partial class NewPasswordPage : UserControl
{
    private readonly Credentials _credentials = new();
    private string? _email;

    public NewPasswordPage(string? email)
    {
        InitializeComponent();
        DataContext = _credentials;
        _email = email;
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new AuthorizationPage("Пароль не был изменен"));
    }

    private async void ChangePasswordButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_credentials.Password is null ||
            _credentials.RepeatPassword is null)
        {
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, "Введите новый пароль");
            return;
        }

        var result = await Users.ChangePassword(_email, _credentials.Password);
        if (!string.IsNullOrEmpty(result))
        {
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, result);
            return;
        }

        MainContent.NavigateTo(new AuthorizationPage("Пароль изменен"));
    }
}