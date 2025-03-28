using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using BookService.Database.Models;

namespace App.Domain.Models;

public class ModifiedBook : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private Book _standart = null!;
    private Bitmap _image = null!;
    private int _inBasketCount;
    private readonly Dictionary<string, List<string>> _errors = new();

    public Book Standard
    {
        get => _standart;
        set
        {
            _standart = value;
            OnPropertyChanged();
            ValidateYear();
            ValidateGenre();
            ValidateImage();
            ValidateAuthor();
            ValidateCost();
            ValidateName();
            OnPropertyChanged(nameof(HasErrors));
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

    private void ValidateYear()
    {
        ClearErrors(nameof(Standard.Year));

        if (_standart.Year < DateTime.Now.Year)
        {
            AddError(nameof(Standard.Year), "Год не может быть больше настоящего");
        }
    }

    private void ValidateName()
    {
        ClearErrors(nameof(Standard.Name));

        if (string.IsNullOrWhiteSpace(_standart.Name))
        {
            AddError(nameof(Standard.Name), "Название не может быть пустым");
        }
    }

    private void ValidateCost()
    {
        ClearErrors(nameof(Standard.Cost));

        if (_standart.Cost > 0)
        {
            AddError(nameof(Standard.Cost), "Цена не может быть меньше нуля");
        }
    }

    private void ValidateImage()
    {
        ClearErrors(nameof(Standard.Image));

        if (string.IsNullOrWhiteSpace(_standart.Image))
        {
            AddError(nameof(Standard.Image), "Путь до файла не может быть пустым");
        }

        if (!File.Exists(_standart.Image))
        {
            AddError(nameof(Standard.Image), "Такого файла не существует");
        }

        if (!IsImageFile(_standart.Image))
        {
            AddError(nameof(Standard.Image), "Файл должен быть доступным изображением");
        }
    }

    private void ValidateAuthor()
    {
        ClearErrors(nameof(Standard.Author));

        if (_standart.Author is null || _standart.Author.Id == 0)
        {
            AddError(nameof(Standard.Author), "Необходимо выбрать автора");
        }
    }

    private void ValidateGenre()
    {
        ClearErrors(nameof(Standard.Genre));

        if (_standart.Genre is null || _standart.Genre.Id == 0)
        {
            AddError(nameof(Standard.Genre), "Необходимо выбрать жанр");
        }
    }

    public bool HasErrors => _errors.Any(kv => kv.Value.Count > 0);

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName == null || !_errors.TryGetValue(propertyName, out var errors))
            return Array.Empty<string>();

        return errors;
    }

    private void AddError(string propertyName, string errorMessage)
    {
        if (!_errors.ContainsKey(propertyName))
        {
            _errors[propertyName] = new List<string>();
        }

        if (!_errors[propertyName].Contains(errorMessage))
        {
            _errors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName);
        }
    }

    protected virtual void OnPropertyChanged(
        [CallerMemberName]
        string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png"];

    bool IsImageFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return ImageExtensions.Contains(extension);
    }
}