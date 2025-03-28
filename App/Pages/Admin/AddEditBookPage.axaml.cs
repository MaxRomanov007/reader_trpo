using App.Domain.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Pages.Admin;

public partial class AddEditBookPage : UserControl
{
    private ModifiedBook _book = new();
    public AddEditBookPage(ModifiedBook? book)
    {
        InitializeComponent();
        if (book != null)
            _book = book;
        DataContext = _book;
    }
}