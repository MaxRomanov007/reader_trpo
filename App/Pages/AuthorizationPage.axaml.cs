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

public partial class AuthorizationPage : UserControl, IMessageShower
{
    private Credentials _Credentials = new();
    public AuthorizationPage()
    {
        InitializeComponent();
        DataContext = _Credentials;
    }

    public AuthorizationPage(string message)
    {
        InitializeComponent();
        DataContext = _Credentials;
        MessageTextBlock.ShowTemporaryText(message);
    }

    public void ShowMessage(string message)
    {
        ErrorTextBlock.ShowTemporaryText(message);
    }

    private async void AuthorizationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_Credentials.Email == null || _Credentials.Password == null)
        {
            ErrorTextBlock.ShowTemporaryText("Введите email и пароль");
            return;
        }
        var isValid = await ConfirmEmailPage.Check(EmailTextBox.Text, this, this);
        Console.WriteLine(isValid);
    }

    private void ToRegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new RegistrationPage());
    }
}