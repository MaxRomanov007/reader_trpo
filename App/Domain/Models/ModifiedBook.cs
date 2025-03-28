using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using BookService.Database.Models;

namespace App.Domain.Models;

public class ModifiedBook : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Book _standart = null!;
    private Bitmap _image = null!;
    private int _inBasketCount;

    public Book Standard
    {
        get => _standart;
        set
        {
            _standart = value;
            OnPropertyChanged();
        }
    }

    public Bitmap Image
    {
        get => _image;
        init
        {
            _image = value;
            OnPropertyChanged();
        }
    }

    public int InBasketCount
    {
        get => _inBasketCount;
        set
        {
            _inBasketCount = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsInBasket));
        }
    }

    public bool IsInBasket => InBasketCount > 0;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}