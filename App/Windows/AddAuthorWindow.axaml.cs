using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BookService;
using BookService.Database.Models;

namespace App.Windows;

public partial class AddAuthorWindow : Window
{
    private Author _author = new();

    public AddAuthorWindow()
    {
        InitializeComponent();

        DataContext = _author;
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_author.Surname)
            || string.IsNullOrWhiteSpace(_author.Name)
            || string.IsNullOrWhiteSpace(_author.Patronymic))
        {
            _author.ValidateAll();
            return;
        }

        await Books.AddAuthor(_author);
        Close();
    }
}