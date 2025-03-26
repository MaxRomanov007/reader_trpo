using System;
using App.Domain.Models;
using App.Domain.Static;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using BookService;
using BookService.Database.models;

namespace App.Pages.User;

public partial class BookDetailsPage : UserControl
{
    private ModifiedBook _book;
    public BookDetailsPage(long id)
    {
        InitializeComponent();

        var book = Books.GetById(id) ?? new Book();
        _book = new ModifiedBook
        {
            Standard = book,
            Image = new Bitmap(book.Image),
            InBasketCount = Orders.GetCountBooksInUserBasket(Session.UserId, id)
        };
        DataContext = _book;
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        await Orders.CreateOrder(Session.UserId, _book.Standard.Id);
        _book.InBasketCount = 1;
    }

    private async void NumericUpDown_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (sender is not NumericUpDown input)
        {
            return;
        }
        if (input.Value is null)
        {
            return;
        }

        var value = decimal.ToInt32(input.Value ?? 0);
        await Orders.ChangeCount(Session.UserId, _book.Standard.Id, value);
        _book.InBasketCount = value;
    }
}