namespace AfrahApp.Services;

public interface IAlertService
{
    Task ShowAsync(string title, string message, string cancel = "OK");
}
