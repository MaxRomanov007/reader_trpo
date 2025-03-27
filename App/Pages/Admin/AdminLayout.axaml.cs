using App.Domain.Static;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace App.Pages.Admin;

public partial class AdminLayout : UserControl
{
    public AdminLayout()
    {
        InitializeComponent();

        AdminContent.Content = AdminContentControl;
        AdminContentControl.Content = new AdminBooksPage();
    }

    private void BooksButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AdminContent.NavigateTo(new AdminBooksPage());
    }
}