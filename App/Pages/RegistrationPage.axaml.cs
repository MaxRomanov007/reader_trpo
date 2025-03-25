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
    private Credentials _Credentials = new();
    public RegistrationPage()
    {
        InitializeComponent();
        DataContext = _Credentials;
    }

    private void RegistrationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_Credentials.Email == null || 
            _Credentials.Password == null ||
            _Credentials.RepeatPassword == null)
        {
            ErrorTextBlock.ShowTemporaryText("Введите данные");
            return;
        }

        var result = Users.Register(_Credentials.Email, _Credentials.Password);
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