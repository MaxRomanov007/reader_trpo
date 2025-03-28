using System.Collections;
using System.ComponentModel;

namespace BookService.Database.Models
{
    public partial class Author : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string _surname = null!;
        private string _name = null!;
        private string _patronymic = null!;
        private readonly Dictionary<string, List<string>> _errors = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public long Id { get; set; }

        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged(nameof(Surname));
                    ValidateSurname();
                }
            }
        }

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

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                if (_patronymic != value)
                {
                    _patronymic = value;
                    OnPropertyChanged(nameof(Patronymic));
                }
            }
        }

        public void ValidateAll()
        {
            ValidateName();
            ValidateSurname();
        }

        public string? Description { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public bool HasErrors => _errors.Any(kv => kv.Value.Count > 0);

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.Values.SelectMany(x => x);

            return _errors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
        }

        private void ValidateSurname()
        {
            ClearErrors(nameof(Surname));

            if (string.IsNullOrWhiteSpace(Surname))
                AddError(nameof(Surname), "Фамилия не может быть пустой");
        }

        private void ValidateName()
        {
            ClearErrors(nameof(Name));

            if (string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), "Имя не может быть пустым");
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
            return obj is Author other && other.Id != 0 && Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}