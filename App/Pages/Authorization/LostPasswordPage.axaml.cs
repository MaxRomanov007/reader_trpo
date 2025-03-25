using System;
using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;

namespace App.Pages.Authorization;

public partial class LostPasswordPage : UserControl
{
    private readonly Credentials _credentials = new();
    
    public LostPasswordPage()
    {
        InitializeComponent();
        DataContext = _credentials;
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new Authorization.AuthorizationPage());
    }

    private async void FindButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_credentials.Email == null)
        {
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, "Введите email");
            return;
        }

        if (!await Users.IsEmailExists(_credentials.Email))
        {
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, "Пользователя с таким email не существует");
            return;
        }

        if (!await Authorization.ConfirmEmailPage.Check(_credentials.Email, this))
        {
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, "Почта не подтверждена");
            return;
        }
        
        Console.WriteLine("hello");
        
        MainContent.NavigateTo(new NewPasswordPage(_credentials.Email));
    }
}