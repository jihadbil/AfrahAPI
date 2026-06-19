using AfrahApp.ViewModels;

namespace AfrahApp.Pages;

public partial class LoginPage : ContentPage
{
    private const string HiddenPasswordGlyph = "\uD83D\uDC41";
    private const string VisiblePasswordGlyph = "\uD83D\uDE48";

    private bool _isPasswordHidden = true;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnTogglePasswordVisibilityClicked(object? sender, EventArgs e)
    {
        _isPasswordHidden = !_isPasswordHidden;
        PasswordEntry.IsPassword = _isPasswordHidden;
        TogglePasswordButton.Text = _isPasswordHidden ? HiddenPasswordGlyph : VisiblePasswordGlyph;
    }
}
