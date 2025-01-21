
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
    private const string FileName = "books.json";
    public ObservableCollection<Book> Books { get; set; } = new();

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public MainViewModel()
    {
        AddCommand = new Command(AddBook);
        EditCommand = new Command<Book>(EditBook);
        DeleteCommand = new Command<Book>(DeleteBook);

        // Подписка на сообщения
        WeakReferenceMessenger.Default.Register<BookMessage>(this, (recipient, message) =>
        {
            if (message.Value != null)
            {
                if (message.IsUpdate)
                {
                    // Обновляем существующую книгу
                    var existingBook = Books.FirstOrDefault(b => b == message.Value);
                    if (existingBook != null)
                    {
                        existingBook.Title = message.Value.Title;
                        existingBook.Author = message.Value.Author;
                    }
                }
                else
                {
                    // Добавляем новую книгу
                    Books.Add(message.Value);
                }

                SaveBooks(); // Сохраняем изменения после добавления/обновления
            }
        });

        // Загружаем книги при старте
        LoadBooks();
    }

    private async void AddBook()
    {
        var book = new Book { Title = string.Empty, Author = string.Empty };
        await Shell.Current.GoToAsync(nameof(AddEditBookPage), true, new Dictionary<string, object>
        {
            { "IsEditing", false },
            { "Book", book }
        });
    }

    private async void EditBook(Book book)
    {
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
        if (book != null)
        {
            Books.Remove(book);
            SaveBooks(); // Сохраняем изменения
        }
    }

    private void SaveBooks()
    {
        try
        {
            var json = JsonSerializer.Serialize(Books);
            var filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось сохранить книги: {ex.Message}", "OK");
        }
    }

    private void LoadBooks()
    {
        try
        {
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
            Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить книги: {ex.Message}", "OK");
        }
    }
}