using AfrahApp.Pages;

namespace AfrahApp;

public partial class AppShell : Shell
{
    public AppShell(
        LoginPage loginPage,
        RegisterPage registerPage,
        MainPage mainPage,
        HallDetailsPage hallDetailsPage)
    {
        InitializeComponent();

        LoginShellContent.Content = loginPage;
        RegisterShellContent.Content = registerPage;
        MainShellContent.Content = mainPage;
        HallDetailsShellContent.Content = hallDetailsPage;
    }
}
