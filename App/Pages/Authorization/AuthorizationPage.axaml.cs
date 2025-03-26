using System;
using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;
using BookService.Utils;

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
        MessageTextBlock.ShowTemporaryText(message);
    }

    private async void AuthorizationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_credentials.Email == null || _credentials.Password == null)
        {
            ErrorTextBlock.ShowTemporaryText("Введите email и пароль");
            return;
        }

        var result = await Users.Authorize(_credentials.Email, _credentials.Password);
        MessageTextBlock.ShowTemporaryText(result);
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