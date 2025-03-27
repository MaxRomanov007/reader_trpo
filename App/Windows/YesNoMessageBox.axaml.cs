using System.Threading.Tasks;
using App.Domain.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace App.Windows;

public partial class YesNoMessageBox : Window
{
    public YesNoMessageBox(string title, string message)
    {
        InitializeComponent();
        DataContext = new MessageBoxContent
        {
            Title = title,
            Message = message
        };
    }

    private void YesButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void NoButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
    
    public static Task<bool> Show(Window parent, string title, string message)
    {
        var msgBox = new YesNoMessageBox(title, message);
        return msgBox.ShowDialog<bool>(parent);
    }
}