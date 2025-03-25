using System.Threading.Tasks;
using App.Domain.Extensions;
using App.Domain.Static;
using App.Domain.Utils;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace App.Pages;

public interface IMessageShower
{
    public void ShowMessage(string message);
}

public partial class ConfirmEmailPage : UserControl
{
    private const int CodeLength = 6;
    
    private readonly UserControl _back;
    private readonly IMessageShower _messageShower;
    private readonly string _code;
    private readonly TaskCompletionSource<bool> _completionSource;

    private ConfirmEmailPage(UserControl back, IMessageShower messageShower, string code, TaskCompletionSource<bool> completionSource)
    {
        InitializeComponent();
        _back = back;
        _messageShower = messageShower;
        _code = code;
        _completionSource = completionSource;
    }
    
    public static async Task<bool> Check(string? email, UserControl back, IMessageShower messageShower)
    {
        var code = Utils.GenerateRandomDigits(CodeLength);
        try
        {
            EmailSender.SendAsync(email, "Проверочный код", $"Ваш проверочный код: {code}");
        }
        catch
        {
            messageShower.ShowMessage("Ошибка отправки письма");
            return false;
        }

        var completionSource = new TaskCompletionSource<bool>();
    
        var form = new ConfirmEmailPage(back, messageShower, code, completionSource);
        MainContent.NavigateTo(form);
    
        return await completionSource.Task;
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _completionSource.SetResult(false);
        MainContent.NavigateTo(_back);
    }

    private void ConfirmButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (CodeTextBox.Text != _code)
        {
            ErrorTextBlock.ShowTemporaryText("Неверный код");
            return;
        }

        _messageShower.ShowMessage("Почта подтверждена");
        _completionSource.SetResult(true);
        MainContent.NavigateTo(_back);
    }
}