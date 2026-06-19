using AfrahApp.Pages;
using AfrahApp.Services;
using AfrahApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AfrahApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        RegisterServices(builder.Services);
        RegisterPagesAndViewModels(builder.Services);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri(GetApiBaseUrl()),
            Timeout = TimeSpan.FromSeconds(20)
        });

        services.AddSingleton<AuthSession>();
        services.AddSingleton<HallDetailsState>();
        services.AddSingleton<IAuthApiClient, AuthApiClient>();
        services.AddSingleton<IHallBookingApiClient, HallBookingApiClient>();
        services.AddSingleton<IAlertService, AlertService>();
        services.AddSingleton<IAppNavigator, AppNavigator>();
    }

    private static void RegisterPagesAndViewModels(IServiceCollection services)
    {
        services.AddSingleton<AppShell>();

        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<HallDetailsViewModel>();

        services.AddTransient<LoginPage>();
        services.AddTransient<RegisterPage>();
        services.AddTransient<MainPage>();
        services.AddTransient<HallDetailsPage>();
    }

    private static string GetApiBaseUrl()
    {
        return DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5026/api/"
            : "http://localhost:5026/api/";
    }
}
