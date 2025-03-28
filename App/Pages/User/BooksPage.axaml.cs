using System.Linq;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using BookService;

namespace App.Pages.User;

public partial class BooksPage : UserControl
{
    public BooksPage()
    {
        InitializeComponent();

        var books = Books.GetAvailableBooks();

        BooksListItemsControl.ItemsSource = books.Select(book => new ModifiedBook
        {
            Standard = book,
            Image = new Bitmap(book.Image)
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (button.Tag is not long id)
        {
            return;
        }

        UserContent.NavigateTo(new BookDetailsPage(id));
    }
}