using AfrahApp.Models.Home;

namespace AfrahApp.Services;

public sealed class HallDetailsState
{
    public HomeHallCard? SelectedHall { get; private set; }

    public void SetSelectedHall(HomeHallCard hallCard)
    {
        SelectedHall = hallCard;
    }
}
