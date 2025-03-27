using App.Domain.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Pages.Admin;

public partial class AddEditBookPage : UserControl
{
    public AddEditBookPage(ModifiedBook? book)
    {
        InitializeComponent();
    }
}