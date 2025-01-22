namespace MauiPTRApp.ViewModels;

using MauiPTRApp.Views;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;
using System.Windows.Input;
using MauiPTRApp.Models;
using MauiPTRApp.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;

public class MainViewModel
{
    private const string FileName = "books.json"; // здесь будем хранить информацию
    public ObservableCollection<Book> Books { get; set; } = new(); // коллекция книг для отображения на странице

    // Команды для управления книгами
    public ICommand AddCommand { get; } // Добавление книги
    public ICommand EditCommand { get; } // Редактирование книги
    public ICommand DeleteCommand { get; } // Удаление книги
    public ICommand ShowDescriptionCommand { get; } // Показ/скрытие описания книги

    public MainViewModel()
    {
        // Инициализация команд
        AddCommand = new Command(AddBook);
        EditCommand = new Command<Book>(EditBook);
        DeleteCommand = new Command<Book>(DeleteBook);
        ShowDescriptionCommand = new Command<Book>(ToggleDescription);

        // Подписка на сообщения для обновления или добавления книг
        WeakReferenceMessenger.Default.Register<BookMessage>(this, (recipient, message) =>
        {
            if (message.Value != null)
            {
                if (message.IsUpdate)
                {
                    // Обновление книги
                    var existingBook = Books.FirstOrDefault(b => b == message.Value);
                    if (existingBook != null)
                    {
                        existingBook.Title = message.Value.Title;
                        existingBook.Author = message.Value.Author;
                        existingBook.Description = message.Value.Description;
                    }
                }
                else
                {
                    // Добавление в коллекцию
                    Books.Add(message.Value);
                }

                SaveBooks(); // Сохранение изменений
            }
        });

        // Загрузка списка книг при запуске
        LoadBooks();
    }

    private async void AddBook()
    {
        // Создание новой книги и открытие страницы для её редактирования
        var book = new Book { Title = string.Empty, Author = string.Empty, Description = string.Empty };
        await Shell.Current.GoToAsync(nameof(AddEditBookPage), true, new Dictionary<string, object>
        {
            { "IsEditing", false },
            { "Book", book }
        });
    }

    private async void EditBook(Book book)
    {
        // Открытие страницы редактирования и передача данных выбранной книги
        if (book != null)
        {
            await Shell.Current.GoToAsync(nameof(AddEditBookPage), true, new Dictionary<string, object>
            {
                { "IsEditing", true },
                { "Book", book }
            });
        }
    }

    private void DeleteBook(Book book)
    {
        // Удаление из коллекции
        if (book != null)
        {
            Books.Remove(book);
            SaveBooks(); //Сохранение изменений
        }
    }

    private void ToggleDescription(Book book)
    {
        // Переключение видимости описания книги
        if (book != null)
        {
            book.IsDescriptionVisible = !book.IsDescriptionVisible;
        }
    }

    private void SaveBooks()
    {
        try
        {
            // Сохраняем колекцию в JSON
            var json = JsonSerializer.Serialize(Books, new JsonSerializerOptions { WriteIndented = true });
            var filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            // Сообщение об ошибке, если сохранение не удалось
            Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось сохранить книги: {ex.Message}", "OK");
        }
    }

    private void LoadBooks()
    {
        try
        {
            // Выгрузка книг из файла JSON, если он существует
            var filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var books = JsonSerializer.Deserialize<ObservableCollection<Book>>(json);
                if (books != null)
                {
                    Books = books;
                }
            }
        }
        catch (Exception ex)
        {
            // Сообщение об ошибке, если загрузка не удалась
            Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить книги: {ex.Message}", "OK");
        }
    }
}