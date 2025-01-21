using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiPTRApp.Models;
using MauiPTRApp.Messages;
using CommunityToolkit.Mvvm.Messaging;
using MauiPTRApp.Views;
using Microsoft.Maui.Controls;

namespace MauiPTRApp.ViewModels;

public class MainViewModel
{
    public ObservableCollection<Book> Books { get; set; } = new();

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public MainViewModel()
    {
        AddCommand = new Command(AddBook);
        EditCommand = new Command<Book>(EditBook);
        DeleteCommand = new Command<Book>(DeleteBook);

        // Подписка на сообщения для добавления или обновления книг
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

                        // Уведомляем об изменении
                        var index = Books.IndexOf(existingBook);
                        Books[index] = existingBook;
                    }
                }
                else
                {
                    // Добавляем новую книгу
                    Books.Add(message.Value);
                }
            }
        });
    }

    private async void AddBook()
    {
        await Shell.Current.GoToAsync(nameof(AddEditBookPage), true, new Dictionary<string, object>
        {
            { "IsEditing", false },
            { "Book", new Book { Title = string.Empty, Author = string.Empty } }
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
        }
    }
}