using System.Collections.Generic;
using System.Linq;
using App.Domain.Models;
using App.Domain.Static;
using App.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using BookService;

namespace App.Pages.User;

public partial class BasketPage : UserControl
{
    private IEnumerable<ModifiedBook>? _books;
    private BasketPageProps? _props;

    public BasketPage()
    {
        InitializeComponent();

        _props = new BasketPageProps
        {
            IsEmpty = false
        };
        DataContext = _props;

        var orderBooks = Orders.GetUserBooksInOrder(Session.UserId);
        _books = orderBooks?.Select(o => new ModifiedBook
        {
            Standard = o.Book,
            Image = new Bitmap(o.Book.Image),
            InBasketCount = o.Count
        });
        var enumerable = _books?.ToList();
        BooksListItemsControl.ItemsSource = enumerable;

        if (enumerable is not null)
        {
            if (enumerable.Count != 0)
            {
                return;
            }
        }

        _props.IsEmpty = true;
    }

    private async void AddToBasketButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (button.Tag is not long id)
        {
            return;
        }

        await Orders.CreateOrder(Session.UserId, id);
        var book = _books?.FirstOrDefault(b => b.Standard.Id == id);
        if (book != null) book.InBasketCount = 1;
    }

    private async void ChangeCountNumericUpDown_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (sender is not NumericUpDown input)
        {
            return;
        }

        if (input.Value is null || input.Tag is not long id)
        {
            return;
        }

        var value = decimal.ToInt32(input.Value ?? 0);
        await Orders.ChangeCount(Session.UserId, id, value);
        var book = _books?.FirstOrDefault(b => b.Standard.Id == id);
        if (book != null) book.InBasketCount = value;
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!await YesNoMessageBox.Show(MainContent.MainWindow, "Оформление заказа",
                "Вы уверены, что хотите оформить заказ?"))
        {
            return;
        }

        BooksListItemsControl.ItemsSource = null;
        if (_props != null) _props.IsEmpty = true;
        await Orders.SendOrder(Session.UserId);
    }
}