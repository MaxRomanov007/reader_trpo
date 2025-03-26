using System;
using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using App.Pages.User;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;
using BookService.Database.models;
using BookService.Utils;

namespace App.Pages.Authorization;

public partial class AuthorizationPage : UserControl
{
    private readonly Credentials _credentials = new();

    public AuthorizationPage()
    {
        InitializeComponent();
        _credentials.Email = "maxromanov4232@gmail.com";
        _credentials.Password = "12345678";
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

        var user = await Users.Authorize(_credentials.Email, _credentials.Password);
        if (user is null)
        {
            ErrorTextBlock.ShowTemporaryText("Неверный логин или пароль");
            return;
        }
        if (user.Role.Name == UserRole.User)
        {
            Session.UserId = user.Id;
            MainContent.NavigateTo(new UserLayout());
        }
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