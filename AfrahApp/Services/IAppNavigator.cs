using AfrahApp.Models.Home;

namespace AfrahApp.Services;

public interface IAppNavigator
{
    Task GoToLoginAsync();
    Task GoToRegisterAsync();
    Task GoToMainAsync();
    Task GoToHallDetailsAsync(HomeHallCard hallCard);
}
