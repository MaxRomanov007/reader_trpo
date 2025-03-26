using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using BookService;

namespace App.Pages.User;

public partial class BooksPage : UserControl
{
    public BooksPage()
    {
        InitializeComponent();

        var books = Books.GetAll();
        books.AddRange(books);
        books.AddRange(books);
        books.AddRange(books);

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