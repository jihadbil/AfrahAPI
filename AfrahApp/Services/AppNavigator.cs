using AfrahApp.Models.Home;

namespace AfrahApp.Services;

public sealed class AppNavigator(HallDetailsState hallDetailsState) : IAppNavigator
{
    public Task GoToLoginAsync()
    {
        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            NavigateByShellContentRoute("LoginPage");
        });
    }

    public Task GoToRegisterAsync()
    {
        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            NavigateByShellContentRoute("RegisterPage");
        });
    }

    public Task GoToMainAsync()
    {
        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            NavigateByShellContentRoute("MainPage");
        });
    }

    public Task GoToHallDetailsAsync(HomeHallCard hallCard)
    {
        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            hallDetailsState.SetSelectedHall(hallCard);
            NavigateByShellContentRoute("HallDetailsPage");
        });
    }

    private static void NavigateByShellContentRoute(string targetRoute)
    {
        var shell = Shell.Current;
        if (shell is null)
        {
            return;
        }

        foreach (var shellItem in shell.Items)
        {
            foreach (var shellSection in shellItem.Items)
            {
                foreach (var shellContent in shellSection.Items)
                {
                    if (!string.Equals(shellContent.Route, targetRoute, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    shellItem.CurrentItem = shellSection;
                    shellSection.CurrentItem = shellContent;
                    shell.CurrentItem = shellItem;
                    return;
                }
            }
        }
    }
}
