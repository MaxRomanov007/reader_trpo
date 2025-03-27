using System.ComponentModel;

namespace App.Domain.Models;

public class BasketPageProps : INotifyPropertyChanged
{
    private bool _isEmpty;
    
    public bool IsEmpty
    {
        get => _isEmpty;
        set
        {
            _isEmpty = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged(
        [System.Runtime.CompilerServices.CallerMemberName]
        string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}