using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

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
            if (_year == value) return;
            _year = value;
            OnPropertyChanged();
            ValidateYear();
        }
    }

    private string _name = null!;
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            OnPropertyChanged();
            ValidateName();
        }
    }

    private decimal _cost;
    public decimal Cost
    {
        get => _cost;
        set
        {
            if (_cost == value) return;
            _cost = value;
            OnPropertyChanged();
            ValidateCost();
        }
    }

    private string _image = null!;
    public string Image
    {
        get => _image;
        set
        {
            if (_image == value) return;
            _image = value;
            OnPropertyChanged();
            ValidateImage();
        }
    }

    private Author _author = null!;
    public virtual Author Author
    {
        get => _author;
        set
        {
            if (Equals(_author, value)) return;
            _author = value;
            AuthorId = value?.Id ?? 0;
            OnPropertyChanged();
            ValidateAuthor();
        }
    }

    private Genre _genre = null!;
    public virtual Genre Genre
    {
        get => _genre;
        set
        {
            if (Equals(_genre, value)) return;
            _genre = value;
            GenreId = value?.Id ?? 0;
            OnPropertyChanged();
            ValidateGenre();
        }
    }

    private string? _description;

    public virtual string? Description
    {
        get => _description;
        set
        {
            if (_description == value) return;
            _description = value;
            OnPropertyChanged();
        }
    }

    public long Id { get; set; }
    public long GenreId { get; set; }
    public long AuthorId { get; set; }
    public long StatusId { get; set; }
    public int Count { get; set; }
    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
    public virtual BookStatus Status { get; set; } = null!;

    #endregion

    #region Validation Methods

    public void ValidateAll()
    {
        ValidateAuthor();
        ValidateGenre();
        ValidateCost();
        ValidateImage();
        ValidateName();
        ValidateYear();
        OnPropertyChanged(nameof(HasErrors));
    }

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

        if (Cost < 0)
        {
            AddError(nameof(Cost), "Цена должна быть больше нуля");
        }
    }

    private void ValidateImage()
    {
        ClearErrors(nameof(Image));

        if (string.IsNullOrWhiteSpace(_image))
        {
            AddError(nameof(Image), "Путь до файла не может быть пустым");
            return;
        }

        if (!File.Exists(_image))
        {
            AddError(nameof(Image), "Такого файла не существует");
        }
        else if (!IsImageFile(_image))
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
        OnPropertyChanged(nameof(HasErrors));
    }

    #endregion

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}