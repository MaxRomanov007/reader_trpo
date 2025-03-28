using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BookService.Database.Models;

public partial class Book : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private readonly Dictionary<string, List<string>> _errors = new();

    #region Properties with validation

    private short _year;
    public short Year
    {
        get => _year;
        set
        {
            if (_year != value)
            {
                _year = value;
                OnPropertyChanged();
                ValidateYear();
            }
        }
    }

    private string _name = null!;
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
                ValidateName();
            }
        }
    }

    private decimal _cost;
    public decimal Cost
    {
        get => _cost;
        set
        {
            if (_cost != value)
            {
                _cost = value;
                OnPropertyChanged();
                ValidateCost();
            }
        }
    }

    private string _image = null!;
    public string Image
    {
        get => _image;
        set
        {
            if (_image != value)
            {
                _image = value;
                OnPropertyChanged();
                ValidateImage();
            }
        }
    }

    private Author _author = null!;
    public virtual Author Author
    {
        get => _author;
        set
        {
            if (_author != value)
            {
                _author = value;
                AuthorId = value?.Id ?? 0;
                OnPropertyChanged();
                ValidateAuthor();
            }
        }
    }

    private Genre _genre = null!;
    public virtual Genre Genre
    {
        get => _genre;
        set
        {
            if (_genre != value)
            {
                _genre = value;
                GenreId = value?.Id ?? 0;
                OnPropertyChanged();
                ValidateGenre();
            }
        }
    }

    // Остальные свойства без валидации остаются как были
    public long Id { get; set; }
    public long GenreId { get; set; }
    public long AuthorId { get; set; }
    public long StatusId { get; set; }
    public int Count { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
    public virtual BookStatus Status { get; set; } = null!;

    #endregion

    #region Validation Methods

    private void ValidateYear()
    {
        ClearErrors(nameof(Year));

        if (Year > DateTime.Now.Year)
        {
            AddError(nameof(Year), "Год не может быть больше настоящего");
        }
    }

    private void ValidateName()
    {
        ClearErrors(nameof(Name));

        if (string.IsNullOrWhiteSpace(Name))
        {
            AddError(nameof(Name), "Название не может быть пустым");
        }
    }

    private void ValidateCost()
    {
        ClearErrors(nameof(Cost));

        if (Cost <= 0)
        {
            AddError(nameof(Cost), "Цена должна быть больше нуля");
        }
    }

    private void ValidateImage()
    {
        ClearErrors(nameof(Image));

        if (string.IsNullOrWhiteSpace(Image))
        {
            AddError(nameof(Image), "Путь до файла не может быть пустым");
            return;
        }

        if (!File.Exists(Image))
        {
            AddError(nameof(Image), "Такого файла не существует");
        }
        else if (!IsImageFile(Image))
        {
            AddError(nameof(Image), "Файл должен быть изображением (jpg, png, etc.)");
        }
    }

    private void ValidateAuthor()
    {
        ClearErrors(nameof(Author));

        if (Author is null || Author.Id == 0)
        {
            AddError(nameof(Author), "Необходимо выбрать автора");
        }
    }

    private void ValidateGenre()
    {
        ClearErrors(nameof(Genre));

        if (Genre is null || Genre.Id == 0)
        {
            AddError(nameof(Genre), "Необходимо выбрать жанр");
        }
    }

    private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

    private bool IsImageFile(string filePath)
    {
        try
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return ImageExtensions.Contains(extension);
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region INotifyDataErrorInfo Implementation

    public bool HasErrors => _errors.Any(kv => kv.Value.Count > 0);

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName == null || !_errors.ContainsKey(propertyName))
            return Array.Empty<string>();

        return _errors[propertyName];
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

    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    #endregion

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}