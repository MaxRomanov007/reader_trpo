using System.Threading.Tasks;
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
        MainContent.NavigateTo(new AuthorizationPage());
    }

    private async void FindButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_credentials.Email is null)
        {
            ErrorTextBlock.ShowTemporaryText("Введите email");
            return;
        }
        
        FindButton.IsEnabled = false;
        await ChangePassword();
        FindButton.IsEnabled = true;
    }

    private async Task ChangePassword()
    {
        if (!await Users.IsEmailExists(_credentials.Email))
        {
            ErrorTextBlock.ShowTemporaryText("Пользователя с таким email не существует");
            return;
        }

        if (!await ConfirmEmailPage.Check(_credentials.Email, this))
        {
            ErrorTextBlock.ShowTemporaryText("Почта не подтверждена");
            return;
        }

        MainContent.NavigateTo(new NewPasswordPage(_credentials.Email));
    }
}