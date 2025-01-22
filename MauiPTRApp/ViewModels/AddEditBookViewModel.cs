namespace MauiPTRApp.ViewModels;

using System.Windows.Input; 
using MauiPTRApp.Models; 
using MauiPTRApp.Messages; 
using CommunityToolkit.Mvvm.Messaging; 
using Microsoft.Maui.Controls; 

[QueryProperty(nameof(IsEditing), "IsEditing")] // Привязка  IsEditing из запроса
[QueryProperty(nameof(Book), "Book")] // Привязка Book из запроса
public class AddEditBookViewModel : BindableObject // ViewModel для страницы добавления/редактирования книги
{
    // Поля для хранения данных о книге
    private Book _book = new Book();
    private string _title = string.Empty;
    private string _author = string.Empty;
    private string _description = string.Empty;

    // Свойство, указывающее, редактируется ли существующая книга
    public bool IsEditing { get; set; }

    // Свойство для доступа к модели Book
    public Book Book
    {
        get => _book;
        set
        {
            _book = value ?? new Book(); // Если значение null, создаем новый объект Book
            Title = _book.Title; // Обновляем свойства книги, автора, описания
            Author = _book.Author;
            Description = _book.Description;
            OnPropertyChanged(); // Уведомляем интерфейс об изменении
        }
    }

    // Редактирование названия
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(); // Уведомляем об изменении значения
        }
    }

    // Редактирование автора
    public string Author
    {
        get => _author;
        set
        {
            _author = value;
            OnPropertyChanged();
        }
    }

    // Редактирование описания
    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged(); // Уведомляем об изменении
            }
        }
    }

    // Команды для сохранения и отмены
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditBookViewModel()
    {
        SaveCommand = new Command(SaveBook); // Инициализация команды сохранения
        CancelCommand = new Command(Cancel); // Инициализация команды отмены
    }

    // Метод сохранения книги
    private async void SaveBook()
    {
        if (Book != null) // Проверяем, существует ли объект КBook
        {
            // Обновляем значения в объекте Book
            Book.Title = Title;
            Book.Author = Author;
            Book.Description = Description; 

            // Отправляем сообщение
            WeakReferenceMessenger.Default.Send(new BookMessage(Book, IsEditing));

            // Переходим к предыдущей странице
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            // Показываем сообщение об ошибке, если объект Book отсутствует
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Книга не найдена", "OK");
        }
    }

    // Отмена редактирования
    private async void Cancel()
    {
        // Возврат к предыдущей странице
        await Shell.Current.GoToAsync("..");
    }
}
