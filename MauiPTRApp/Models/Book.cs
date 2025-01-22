using System.ComponentModel;

namespace MauiPTRApp.Models
{
    public class Book : INotifyPropertyChanged
    {
        // Название книги
        private string _title = string.Empty;
        // Автор книги
        private string _author = string.Empty;
        // Описание
        private string _description = string.Empty;
        // Открывашка информации о книге
        private bool _isDescriptionVisible;

        // Получение названия
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value) // Проверка на изменение значения
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title)); // Уведомление об изменении свойства
                }
            }
        }

        // Получение автора
        public string Author
        {
            get => _author;
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged(nameof(Author)); // Уведомление об изменении свойства
                }
            }
        }

        // Описание книги
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description)); // Уведомление об изменении свойства
                }
            }
        }

        // Видимость описания книги
        public bool IsDescriptionVisible
        {
            get => _isDescriptionVisible;
            set
            {
                if (_isDescriptionVisible != value)
                {
                    _isDescriptionVisible = value;
                    OnPropertyChanged(nameof(IsDescriptionVisible)); // Уведомление об изменении свойства
                }
            }
        }

        // Эвент уведомления об изменении свойств
        public event PropertyChangedEventHandler? PropertyChanged;

        // Метод для вызова события
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
