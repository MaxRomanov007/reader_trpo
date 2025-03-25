using System;
using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using App.Domain.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BookService;
using BookService.Database.models;

namespace App.Pages;

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

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new LostPasswordPage());
    }
}