using System.Net.Mail;
using System.Text.RegularExpressions;
using AfrahApp.Infrastructure;
using AfrahApp.Models.Auth;
using AfrahApp.Services;
using Microsoft.Maui.Controls;

namespace AfrahApp.ViewModels;

public sealed class RegisterViewModel : BaseViewModel
{
    private readonly IAuthApiClient _authApiClient;
    private readonly IAlertService _alertService;
    private readonly IAppNavigator _appNavigator;

    private string _fullName = string.Empty;
    private string _email = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _password = string.Empty;
    private bool _isTermsAccepted;
    private bool _isPasswordHidden = true;
    private string _registerButtonText = "إنشاء حساب";

    public RegisterViewModel(
        IAuthApiClient authApiClient,
        IAlertService alertService,
        IAppNavigator appNavigator)
    {
        _authApiClient = authApiClient;
        _alertService = alertService;
        _appNavigator = appNavigator;

        RegisterCommand = new AsyncCommand(RegisterAsync, () => IsNotBusy);
        BackToLoginCommand = new AsyncCommand(BackToLoginAsync, () => IsNotBusy);
        TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (SetProperty(ref _fullName, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (SetProperty(ref _phoneNumber, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public bool IsTermsAccepted
    {
        get => _isTermsAccepted;
        set
        {
            if (SetProperty(ref _isTermsAccepted, value))
            {
                RaiseCommandStates();
            }
        }
    }

    public bool IsPasswordHidden
    {
        get => _isPasswordHidden;
        set
        {
            if (SetProperty(ref _isPasswordHidden, value))
            {
                OnPropertyChanged(nameof(PasswordToggleIcon));
            }
        }
    }

    public string PasswordToggleIcon => IsPasswordHidden ? "⌧" : "◉";

    public string RegisterButtonText
    {
        get => _registerButtonText;
        private set => SetProperty(ref _registerButtonText, value);
    }

    public AsyncCommand RegisterCommand { get; }
    public AsyncCommand BackToLoginCommand { get; }
    public Command TogglePasswordVisibilityCommand { get; }

    private async Task RegisterAsync()
    {
        if (!ValidateInput(out var validationError))
        {
            await _alertService.ShowAsync("تنبيه", validationError);
            return;
        }

        IsBusy = true;
        RegisterButtonText = "جاري إنشاء الحساب...";
        RaiseCommandStates();

        try
        {
            var (firstName, lastName) = SplitFullName(FullName.Trim());
            var normalizedEmail = Email.Trim();

            var result = await _authApiClient.RegisterAsync(new RegisterRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = normalizedEmail,
                UserName = BuildUserName(normalizedEmail),
                PhoneNumber = NormalizePhoneNumber(PhoneNumber),
                DateOfBirth = DateTime.Today.AddYears(-20),
                Address = null,
                Password = Password,
                ConfirmPassword = Password
            });

            if (!result.IsSuccess)
            {
                await _alertService.ShowAsync("فشل التسجيل", result.Message);
                return;
            }

            await _alertService.ShowAsync("تم بنجاح", "تم إنشاء الحساب بنجاح. يمكنك تسجيل الدخول الآن.");
            await _appNavigator.GoToLoginAsync();
            ClearSensitiveFields();
        }
        finally
        {
            IsBusy = false;
            RegisterButtonText = "إنشاء حساب";
            RaiseCommandStates();
        }
    }

    private Task BackToLoginAsync()
    {
        return _appNavigator.GoToLoginAsync();
    }

    private void TogglePasswordVisibility()
    {
        IsPasswordHidden = !IsPasswordHidden;
    }

    private bool ValidateInput(out string message)
    {
        if (string.IsNullOrWhiteSpace(FullName))
        {
            message = "يرجى إدخال الاسم الكامل.";
            return false;
        }

        if (!IsValidEmail(Email))
        {
            message = "يرجى إدخال بريد إلكتروني صحيح.";
            return false;
        }

        if (!IsValidPhone(PhoneNumber))
        {
            message = "يرجى إدخال رقم هاتف صحيح.";
            return false;
        }

        if (!IsStrongPassword(Password))
        {
            message = "كلمة المرور يجب أن تحتوي 8 أحرف على الأقل، وحرف كبير وحرف صغير ورقم ورمز خاص.";
            return false;
        }

        if (!IsTermsAccepted)
        {
            message = "يجب الموافقة على الشروط والأحكام لإكمال التسجيل.";
            return false;
        }

        message = string.Empty;
        return true;
    }

    private void ClearSensitiveFields()
    {
        Password = string.Empty;
    }

    private void RaiseCommandStates()
    {
        RegisterCommand.RaiseCanExecuteChanged();
        BackToLoginCommand.RaiseCanExecuteChanged();
    }

    private static bool IsValidEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        try
        {
            _ = new MailAddress(value.Trim());
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = NormalizePhoneNumber(value);
        return Regex.IsMatch(normalized, @"^\+?\d{9,15}$");
    }

    private static bool IsStrongPassword(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 8)
        {
            return false;
        }

        var hasUpper = value.Any(char.IsUpper);
        var hasLower = value.Any(char.IsLower);
        var hasDigit = value.Any(char.IsDigit);
        var hasSymbol = value.Any(ch => !char.IsLetterOrDigit(ch));

        return hasUpper && hasLower && hasDigit && hasSymbol;
    }

    private static (string FirstName, string LastName) SplitFullName(string fullName)
    {
        var parts = fullName
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return parts.Length switch
        {
            0 => ("مستخدم", "جديد"),
            1 => (parts[0], "مستخدم"),
            _ => (parts[0], string.Join(" ", parts.Skip(1)))
        };
    }

    private static string NormalizePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return string.Empty;
        }

        var trimmed = phoneNumber.Trim().Replace(" ", string.Empty).Replace("-", string.Empty);
        if (trimmed.StartsWith("00", StringComparison.Ordinal))
        {
            trimmed = $"+{trimmed[2..]}";
        }

        return trimmed;
    }

    private static string BuildUserName(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return $"user{DateTime.UtcNow:HHmmss}";
        }

        return Regex.Replace(normalized, @"[^a-zA-Z0-9\-._@+]", string.Empty);
    }
}
