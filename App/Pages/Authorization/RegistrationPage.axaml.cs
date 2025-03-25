using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;

namespace App.Pages.Authorization;

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
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, "Введите данные");
            return;
        }

        if (!await Authorization.ConfirmEmailPage.Check(_credentials.Email, this))
        {
            return;
        }

        var result = await Users.Register(_credentials.Email, _credentials.Password);
        if (result != string.Empty)
        {
            TextBlockExtensions.ShowTemporaryText(ErrorTextBlock, result);
            return;
        }
        
        MainContent.NavigateTo(new Authorization.AuthorizationPage("Вы успешно зарегистрированы"));
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.NavigateTo(new Authorization.AuthorizationPage());
    }
}