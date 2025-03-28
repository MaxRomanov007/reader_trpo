using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;
using BookService.Database.Models;

namespace App.Windows;

public partial class AddGenreWindow : Window
{
    private Genre _genre = new();

    public AddGenreWindow()
    {
        InitializeComponent();

        DataContext = _genre;
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_genre.Name))
        {
            _genre.ValidateName();
            return;
        }

        await Books.AddGenre(_genre);
        Close();
    }
}