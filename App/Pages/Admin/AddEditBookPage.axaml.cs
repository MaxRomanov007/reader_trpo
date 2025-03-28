using System;
using App.Domain.Models;
using App.Domain.Static;
using App.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;
using BookService.Database.Models;

namespace App.Pages.Admin;

public partial class AddEditBookPage : UserControl
{
    private ModifiedBook _book = new();

    public AddEditBookPage(ModifiedBook? book)
    {
        InitializeComponent();
        AuthorComboBox.ItemsSource = Books.GetAuthors();
        GenreComboBox.ItemsSource = Books.GetGenres();

        _book.Standard = new Book();

        if (book != null)
            _book = book;

        DataContext = _book;
        _book.Standard.ValidateAll();
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_book.Standard.Id == 0)
        {
            await Books.SaveBook(_book.Standard);
        }
        else
        {
            await Books.UpdateBook(_book.Standard);
        }

        AdminContent.NavigateTo(new AdminBooksPage());
    }

    private async void AddGenreButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await new AddGenreWindow().ShowDialog(MainContent.MainWindow);
        GenreComboBox.ItemsSource = Books.GetGenres();
    }

    private async void AddAuthorButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await new AddAuthorWindow().ShowDialog(MainContent.MainWindow);
        AuthorComboBox.ItemsSource = Books.GetAuthors();
    }

    [Obsolete("Obsolete")]
    private async void FindSourceButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var ofd = new OpenFileDialog
        {
            Title = "Выберите Изображение",
            Filters =
            [
                new FileDialogFilter { Name = "Изображения", Extensions = { "jpg", "jpeg", "png" } },
                new FileDialogFilter { Name = "Все файлы", Extensions = { "*" } }
            ]
        };
        var result = await ofd.ShowAsync(MainContent.MainWindow);
        if (result is not { Length: > 0 })
        {
            return;
        }

        _book.Standard.Image = result[0];
    }
}