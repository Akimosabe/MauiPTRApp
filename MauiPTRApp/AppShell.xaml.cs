using MauiPTRApp.Views;

namespace MauiPTRApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Регистрация маршрута
        Routing.RegisterRoute(nameof(AddEditBookPage), typeof(AddEditBookPage));
    }
}
