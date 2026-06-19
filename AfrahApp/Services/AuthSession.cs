namespace AfrahApp.Services;

public sealed class AuthSession
{
    private const string TokenKey = "auth_token";
    private const string UserNameKey = "auth_username";
    private const string EmailKey = "auth_email";
    private const string UserIdKey = "auth_user_id";

    public async Task SaveAsync(string token, string userName, string email, Guid userId)
    {
        await SecureStorage.Default.SetAsync(TokenKey, token);
        Preferences.Default.Set(UserNameKey, userName);
        Preferences.Default.Set(EmailKey, email);
        Preferences.Default.Set(UserIdKey, userId.ToString("D"));
    }

    public Task<string?> GetTokenAsync()
    {
        return SecureStorage.Default.GetAsync(TokenKey);
    }

    public string GetUserName()
    {
        return Preferences.Default.Get(UserNameKey, string.Empty);
    }

    public string GetEmail()
    {
        return Preferences.Default.Get(EmailKey, string.Empty);
    }

    public Guid GetUserId()
    {
        var value = Preferences.Default.Get(UserIdKey, string.Empty);
        return Guid.TryParse(value, out var userId) ? userId : Guid.Empty;
    }

    public void Clear()
    {
        SecureStorage.Default.Remove(TokenKey);
        Preferences.Default.Remove(UserNameKey);
        Preferences.Default.Remove(EmailKey);
        Preferences.Default.Remove(UserIdKey);
    }
}
