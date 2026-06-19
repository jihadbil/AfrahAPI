namespace AfrahApp.Services;

public sealed class AlertService : IAlertService
{
    public Task ShowAsync(string title, string message, string cancel = "OK")
    {
        return MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var currentPage = Shell.Current?.CurrentPage
                ?? Application.Current?.Windows.FirstOrDefault()?.Page;

            if (currentPage is not null)
            {
                await currentPage.DisplayAlertAsync(title, message, cancel);
            }
        });
    }
}
