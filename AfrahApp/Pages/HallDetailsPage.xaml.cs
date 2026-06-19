using AfrahApp.ViewModels;

namespace AfrahApp.Pages;

public partial class HallDetailsPage : ContentPage
{
    private readonly HallDetailsViewModel _viewModel;

    public HallDetailsPage(HallDetailsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
