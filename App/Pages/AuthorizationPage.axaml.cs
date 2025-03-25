using System;
using App.Domain.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BookService;
using BookService.Database.models;

namespace App.Pages;

public partial class AuthorizationPage : UserControl
{
    private Credentials _Credentials = new Credentials();
    public AuthorizationPage()
    {
        InitializeComponent();
        DataContext = _Credentials;
    }

    private void AuthorizationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_Credentials.Email == null || _Credentials.Password == null)
        {
            ErrorTextBlock.Text = "Введите email и пароль";
            return;
        }
    }
}