using System.Windows.Input;
using MauiPTRApp.Models;
using MauiPTRApp.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;

namespace MauiPTRApp.ViewModels;

[QueryProperty(nameof(IsEditing), "IsEditing")]
[QueryProperty(nameof(Book), "Book")]
public class AddEditBookViewModel : BindableObject
{
    private Book _book = new Book();
    private string _title = string.Empty;
    private string _author = string.Empty;

    public bool IsEditing { get; set; }

    public Book Book
    {
        get => _book;
        set
        {
            _book = value ?? new Book();
            Title = _book.Title;
            Author = _book.Author;
            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public string Author
    {
        get => _author;
        set
        {
            _author = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditBookViewModel()
    {
        SaveCommand = new Command(SaveBook);
        CancelCommand = new Command(Cancel);
    }

    private async void SaveBook()
    {
        if (Book != null)
        {
            Book.Title = Title;
            Book.Author = Author;

            // Отправляем сообщение о сохранении книги
            WeakReferenceMessenger.Default.Send(new BookMessage(Book, IsEditing));

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Книга не найдена", "OK");
        }
    }

    private async void Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}