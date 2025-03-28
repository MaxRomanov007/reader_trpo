using System.Collections;
using System.ComponentModel;

namespace BookService.Database.Models
{
    public partial class Genre : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string _name;
        private readonly Dictionary<string, List<string>> _errors = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public long Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                    ValidateName();
                }
            }
        }

        public string? Description { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public bool HasErrors => _errors.Any(kv => kv.Value.Count > 0);

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
                return Enumerable.Empty<string>();

            return _errors[propertyName];
        }

        public void ValidateName()
        {
            ClearErrors(nameof(Name));

            if (string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), "Название жанра не может быть пустым");
        }

        private void AddError(string propertyName, string errorMessage)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(errorMessage))
            {
                _errors[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        public override bool Equals(object? obj)
        {
            return obj is Genre other && other.Id != 0 && Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}