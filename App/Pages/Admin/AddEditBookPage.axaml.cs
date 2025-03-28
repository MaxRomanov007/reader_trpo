using App.Domain.Models;
using App.Domain.Static;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
}