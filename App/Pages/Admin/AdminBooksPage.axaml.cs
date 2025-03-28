using System.Linq;
using App.Domain.Models;
using App.Domain.Static;
using App.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using BookService;

namespace App.Pages.Admin;

public partial class AdminBooksPage : UserControl
{
    public AdminBooksPage()
    {
        InitializeComponent();

        UpdateSource();
    }

    private void UpdateSource()
    {
        var books = Books.GetAllWithRelative();

        BooksDataGrid.ItemsSource = books.Select(b => new ModifiedBook
        {
            Standard = b,
            Image = new Bitmap(b.Image)
        });
    }

    private async void DeleteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var selectedItems = BooksDataGrid.SelectedItems.Cast<ModifiedBook>().ToList();

        if (!await YesNoMessageBox.Show(
                MainContent.MainWindow,
                "Удаление книг",
                $"Вы точно хотите удалить следующие {selectedItems.Count} записей"))
        {
            return;
        }

        await Books.RemoveRange(selectedItems.Select(i => i.Standard).ToList());
        UpdateSource();
    }

    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AdminContent.NavigateTo(new AddEditBookPage(null));
    }

    private void EditButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        AdminContent.NavigateTo(new AddEditBookPage(button.DataContext as ModifiedBook));
    }
}