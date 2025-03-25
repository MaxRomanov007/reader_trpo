using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace App.Domain.Models;

public class Credentials : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private string? _email;
    private string? _password;
    private readonly Dictionary<string, List<string>> _errors = new();

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public string? Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
            ValidateEmail();
        }
    }

    public string? Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ValidatePassword();
        }
    }

    public bool HasErrors => _errors.Count > 0;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName == null || !_errors.ContainsKey(propertyName))
            return Array.Empty<string>();

        return _errors[propertyName];
    }

    private void ValidateEmail()
    {
        ClearErrors(nameof(Email));

        if (string.IsNullOrWhiteSpace(Email))
        {
            AddError(nameof(Email), "Необходимо ввести Email");
        }
        else if (!Utils.Utils.IsValidEmail(Email))
        {
            AddError(nameof(Email), "Некорректный Email");
        }
    }

    private void ValidatePassword()
    {
        ClearErrors(nameof(Password));

        if (string.IsNullOrWhiteSpace(Password))
        {
            AddError(nameof(Password), "Необходимо ввести пароль");
        }
        else if (Password.Length < 8)
        {
            AddError(nameof(Password), "Пароль должен быть длиннее 8 символов");
        }
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
        [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}