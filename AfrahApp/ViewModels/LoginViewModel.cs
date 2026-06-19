using AfrahApp.Infrastructure;
using AfrahApp.Models.Auth;
using AfrahApp.Services;

namespace AfrahApp.ViewModels;

public sealed class LoginViewModel : BaseViewModel
{
    private readonly IAuthApiClient _authApiClient;
    private readonly AuthSession _authSession;
    private readonly IAlertService _alertService;
    private readonly IAppNavigator _appNavigator;

    private string _emailOrUserName = string.Empty;
    private string _password = string.Empty;
    private string _loginButtonText = "تسجيل الدخول";

    public LoginViewModel(
        IAuthApiClient authApiClient,
        AuthSession authSession,
        IAlertService alertService,
        IAppNavigator appNavigator)
    {
        _authApiClient = authApiClient;
        _authSession = authSession;
        _alertService = alertService;
        _appNavigator = appNavigator;

        LoginCommand = new AsyncCommand(LoginAsync, () => IsNotBusy);
        GoToRegisterCommand = new AsyncCommand(GoToRegisterAsync, () => IsNotBusy);
    }

    public string EmailOrUserName
    {
        get => _emailOrUserName;
        set
        {
            if (SetProperty(ref _emailOrUserName, value))
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

    public string LoginButtonText
    {
        get => _loginButtonText;
        private set => SetProperty(ref _loginButtonText, value);
    }

    public AsyncCommand LoginCommand { get; }
    public AsyncCommand GoToRegisterCommand { get; }

    private async Task LoginAsync()
    {
        var emailOrUserName = EmailOrUserName.Trim();
        var password = Password;

        if (string.IsNullOrWhiteSpace(emailOrUserName) || string.IsNullOrWhiteSpace(password))
        {
            await _alertService.ShowAsync("Validation", "Email/username and password are required.");
            return;
        }

        IsBusy = true;
        LoginButtonText = "جاري تسجيل الدخول...";
        RaiseCommandStates();

        try
        {
            var result = await _authApiClient.LoginAsync(new LoginRequest
            {
                EmailOrUserName = emailOrUserName,
                Password = password
            });

            if (!result.IsSuccess || result.Data is null)
            {
                await _alertService.ShowAsync("Login failed", result.Message);
                return;
            }

            await _authSession.SaveAsync(
                result.Data.Token,
                result.Data.UserInfo?.UserName ?? emailOrUserName,
                result.Data.UserInfo?.Email ?? string.Empty,
                result.Data.UserInfo?.UserId ?? Guid.Empty);

            Password = string.Empty;
            await _appNavigator.GoToMainAsync();
        }
        finally
        {
            IsBusy = false;
            LoginButtonText = "تسجيل الدخول";
            RaiseCommandStates();
        }
    }

    private Task GoToRegisterAsync()
    {
        return _appNavigator.GoToRegisterAsync();
    }

    private void RaiseCommandStates()
    {
        LoginCommand.RaiseCanExecuteChanged();
        GoToRegisterCommand.RaiseCanExecuteChanged();
    }
}
