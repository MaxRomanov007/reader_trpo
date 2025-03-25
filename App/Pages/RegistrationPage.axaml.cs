using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BookService;

namespace App.Pages;

public partial class RegistrationPage : UserControl
{
    private readonly Credentials _credentials = new();
    public RegistrationPage()
    {
        InitializeComponent();
        DataContext = _credentials;
    }

    private async void RegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_credentials.Email == null || 
            _credentials.Password == null ||
            _credentials.RepeatPassword == null)
        {
            ErrorTextBlock.ShowTemporaryText("Введите данные");
            return;
        }

        if (!await ConfirmEmailPage.Check(_credentials.Email, this))
        {
            return;
        }

        var result = await Users.Register(_credentials.Email, _credentials.Password);
        if (result != string.Empty)
        {
            ErrorTextBlock.ShowTemporaryText(result);
            return;
        }
        
        MainContent.NavigateTo(new AuthorizationPage("Вы успешно зарегистрированы"));
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new AuthorizationPage());
    }
}