using System.Collections.ObjectModel;
using System.Globalization;
using AfrahApp.Infrastructure;
using AfrahApp.Models.Common;
using AfrahApp.Models.Hall;
using AfrahApp.Models.Home;
using AfrahApp.Services;

namespace AfrahApp.ViewModels;

public sealed class HallDetailsViewModel : BaseViewModel
{
    private readonly HallDetailsState _hallDetailsState;
    private readonly IHallBookingApiClient _hallBookingApiClient;
    private readonly IAppNavigator _appNavigator;
    private readonly IAlertService _alertService;

    private string _hallName = "تفاصيل الصالة";
    private string _location = "العنوان غير متوفر";
    private string _ratingText = "—";
    private string _reviewsCountText = "لا توجد تقييمات";
    private string _ratingsSummaryText = "لا توجد تقييمات موثقة لهذه الصالة.";
    private string _availabilityText = "غير معروف";
    private string _availabilityBackgroundColor = "#F1ECEE";
    private string _availabilityTextColor = "#8A6E77";
    private string _capacityRangeText = "غير متوفر";
    private string _priceValueText = "غير متوفر";
    private string _aboutText = "لا يوجد وصف متاح لهذه الصالة.";
    private string _mapImageUrl = string.Empty;
    private string _galleryStatusText = "لا توجد صور متاحة لهذه الصالة.";
    private string _servicesStatusText = "لا توجد خدمات مسجلة لهذه الصالة.";
    private string _favoriteIcon = "❤";
    private string _favoriteIconColor = "#E6195D";
    private bool _isFavorite = true;

    public HallDetailsViewModel(
        HallDetailsState hallDetailsState,
        IHallBookingApiClient hallBookingApiClient,
        IAppNavigator appNavigator,
        IAlertService alertService)
    {
        _hallDetailsState = hallDetailsState;
        _hallBookingApiClient = hallBookingApiClient;
        _appNavigator = appNavigator;
        _alertService = alertService;

        GalleryImages = [];
        Services = [];

        BackCommand = new AsyncCommand(BackAsync, () => IsNotBusy);
        ToggleFavoriteCommand = new AsyncCommand(ToggleFavoriteAsync, () => IsNotBusy);
        BookNowCommand = new AsyncCommand(BookNowAsync, () => IsNotBusy);
    }

    public ObservableCollection<string> GalleryImages { get; }
    public ObservableCollection<string> Services { get; }

    public AsyncCommand BackCommand { get; }
    public AsyncCommand ToggleFavoriteCommand { get; }
    public AsyncCommand BookNowCommand { get; }

    public string HallName
    {
        get => _hallName;
        private set => SetProperty(ref _hallName, value);
    }

    public string Location
    {
        get => _location;
        private set => SetProperty(ref _location, value);
    }

    public string RatingText
    {
        get => _ratingText;
        private set => SetProperty(ref _ratingText, value);
    }

    public string ReviewsCountText
    {
        get => _reviewsCountText;
        private set => SetProperty(ref _reviewsCountText, value);
    }

    public string RatingsSummaryText
    {
        get => _ratingsSummaryText;
        private set => SetProperty(ref _ratingsSummaryText, value);
    }

    public string AvailabilityText
    {
        get => _availabilityText;
        private set => SetProperty(ref _availabilityText, value);
    }

    public string AvailabilityBackgroundColor
    {
        get => _availabilityBackgroundColor;
        private set => SetProperty(ref _availabilityBackgroundColor, value);
    }

    public string AvailabilityTextColor
    {
        get => _availabilityTextColor;
        private set => SetProperty(ref _availabilityTextColor, value);
    }

    public string CapacityRangeText
    {
        get => _capacityRangeText;
        private set => SetProperty(ref _capacityRangeText, value);
    }

    public string PriceValueText
    {
        get => _priceValueText;
        private set => SetProperty(ref _priceValueText, value);
    }

    public string AboutText
    {
        get => _aboutText;
        private set => SetProperty(ref _aboutText, value);
    }

    public string MapImageUrl
    {
        get => _mapImageUrl;
        private set
        {
            if (SetProperty(ref _mapImageUrl, value))
            {
                OnPropertyChanged(nameof(IsMapVisible));
            }
        }
    }

    public string FavoriteIcon
    {
        get => _favoriteIcon;
        private set => SetProperty(ref _favoriteIcon, value);
    }

    public string FavoriteIconColor
    {
        get => _favoriteIconColor;
        private set => SetProperty(ref _favoriteIconColor, value);
    }

    public string GalleryStatusText
    {
        get => _galleryStatusText;
        private set
        {
            if (SetProperty(ref _galleryStatusText, value))
            {
                OnPropertyChanged(nameof(IsGalleryStatusVisible));
            }
        }
    }

    public string ServicesStatusText
    {
        get => _servicesStatusText;
        private set
        {
            if (SetProperty(ref _servicesStatusText, value))
            {
                OnPropertyChanged(nameof(IsServicesStatusVisible));
            }
        }
    }

    public bool IsMapVisible => !string.IsNullOrWhiteSpace(MapImageUrl);
    public bool IsGalleryStatusVisible => !string.IsNullOrWhiteSpace(GalleryStatusText);
    public bool IsServicesStatusVisible => !string.IsNullOrWhiteSpace(ServicesStatusText);

    public async Task LoadAsync()
    {
        var hallCard = _hallDetailsState.SelectedHall;

        if (hallCard is null)
        {
            ApplyUnavailableState();
            return;
        }

        IsBusy = true;
        RaiseCommandStates();

        try
        {
            var hallTask = _hallBookingApiClient.GetHallByIdAsync(hallCard.HallId);
            var ratingsTask = _hallBookingApiClient.GetHallRatingsAsync(hallCard.HallId);
            var servicesTask = _hallBookingApiClient.GetHallServicesAsync(hallCard.HallId);
            var mediaTask = _hallBookingApiClient.GetHallMediaAsync(hallCard.HallId);

            await Task.WhenAll(hallTask, ratingsTask, servicesTask, mediaTask);

            var hallResult = await hallTask;
            var hall = hallResult.IsSuccess && hallResult.Data is not null
                ? hallResult.Data
                : null;

            ApplyHall(hall, hallCard);
            ApplyRatings(await ratingsTask);
            ApplyServices(await servicesTask);
            ApplyGallery(await mediaTask, hall, hallCard);
        }
        finally
        {
            IsBusy = false;
            RaiseCommandStates();
        }
    }

    private Task BackAsync()
    {
        return _appNavigator.GoToMainAsync();
    }

    private Task ToggleFavoriteAsync()
    {
        _isFavorite = !_isFavorite;
        FavoriteIcon = _isFavorite ? "❤" : "♡";
        FavoriteIconColor = _isFavorite ? "#E6195D" : "#8A6E77";
        return Task.CompletedTask;
    }

    private Task BookNowAsync()
    {
        return _alertService.ShowAsync("الحجز", $"تم اختيار {HallName}. سيتم تفعيل الحجز في الخطوة التالية.");
    }

    private void ApplyHall(HallReadResponse? hall, HomeHallCard hallCard)
    {
        HallName = hall?.HallName?.Trim() is { Length: > 0 }
            ? hall.HallName
            : (string.IsNullOrWhiteSpace(hallCard.Name) ? "تفاصيل الصالة" : hallCard.Name);

        Location = hall?.Address?.Trim() is { Length: > 0 }
            ? hall.Address
            : (string.IsNullOrWhiteSpace(hallCard.Location) ? "العنوان غير متوفر" : hallCard.Location);

        var isAvailable = hall?.IsAvailable ?? hallCard.IsAvailable;
        AvailabilityText = isAvailable ? "متاح للحجز" : "غير متاح";
        AvailabilityBackgroundColor = isAvailable ? "#FCE7EF" : "#F1ECEE";
        AvailabilityTextColor = isAvailable ? "#E6195D" : "#8A6E77";

        CapacityRangeText = hall?.Capacity > 0
            ? $"{hall.Capacity:N0} شخص"
            : BuildCapacityFromCard(hallCard.CapacityText);

        PriceValueText = BuildPriceText(hall, hallCard.PriceValueText);
        AboutText = hall?.Description?.Trim() is { Length: > 0 }
            ? hall.Description
            : "لا يوجد وصف متاح لهذه الصالة.";

        MapImageUrl = BuildMapImageUrl(hall?.Latitude ?? 0, hall?.Longitude ?? 0);
    }

    private void ApplyRatings(ApiResult<IReadOnlyList<HallRatingReadResponse>> ratingsResult)
    {
        var ratings = ratingsResult.IsSuccess ? ratingsResult.Data : null;

        if (ratings is null || ratings.Count == 0)
        {
            RatingText = "—";
            ReviewsCountText = "لا توجد تقييمات";
            RatingsSummaryText = "لا توجد تقييمات موثقة لهذه الصالة.";
            return;
        }

        var normalizedRatings = ratings
            .Select(item => Math.Clamp(item.OverallRating, 1, 5))
            .ToList();

        var average = normalizedRatings.Average();
        RatingText = average.ToString("0.0", CultureInfo.InvariantCulture);
        ReviewsCountText = $"{normalizedRatings.Count:N0} تقييم";
        RatingsSummaryText = $"{normalizedRatings.Count:N0} تقييم موثق لهذه الصالة.";
    }

    private void ApplyServices(ApiResult<IReadOnlyList<HallServiceReadResponse>> servicesResult)
    {
        Services.Clear();

        if (!servicesResult.IsSuccess || servicesResult.Data is null)
        {
            ServicesStatusText = "تعذر تحميل خدمات الصالة حالياً.";
            return;
        }

        foreach (var service in servicesResult.Data
                     .Where(s => !string.IsNullOrWhiteSpace(s.ServiceName))
                     .Select(s => s.ServiceName.Trim())
                     .Distinct(StringComparer.OrdinalIgnoreCase)
                     .OrderBy(name => name))
        {
            Services.Add(service);
        }

        ServicesStatusText = Services.Count == 0
            ? "لا توجد خدمات مسجلة لهذه الصالة."
            : string.Empty;
    }

    private void ApplyGallery(
        ApiResult<IReadOnlyList<HallMediaReadResponse>> mediaResult,
        HallReadResponse? hall,
        HomeHallCard hallCard)
    {
        GalleryImages.Clear();

        if (mediaResult.IsSuccess && mediaResult.Data is not null)
        {
            foreach (var mediaPath in mediaResult.Data
                         .OrderByDescending(item => item.IsMain)
                         .ThenBy(item => item.DisplayOrder)
                         .Select(item => item.MediaPath)
                         .Where(IsHttpUrl))
            {
                if (GalleryImages.Contains(mediaPath))
                {
                    continue;
                }

                GalleryImages.Add(mediaPath);
            }
        }

        if (IsHttpUrl(hall?.MainImageUrl) && !GalleryImages.Contains(hall!.MainImageUrl!))
        {
            GalleryImages.Insert(0, hall.MainImageUrl!);
        }

        if (IsHttpUrl(hallCard.ImageUrl) && !GalleryImages.Contains(hallCard.ImageUrl))
        {
            GalleryImages.Add(hallCard.ImageUrl);
        }

        GalleryStatusText = GalleryImages.Count == 0
            ? "لا توجد صور متاحة لهذه الصالة."
            : string.Empty;
    }

    private void ApplyUnavailableState()
    {
        HallName = "تفاصيل الصالة";
        Location = "العنوان غير متوفر";
        RatingText = "—";
        ReviewsCountText = "لا توجد تقييمات";
        RatingsSummaryText = "لا توجد تقييمات موثقة لهذه الصالة.";
        AvailabilityText = "غير معروف";
        AvailabilityBackgroundColor = "#F1ECEE";
        AvailabilityTextColor = "#8A6E77";
        CapacityRangeText = "غير متوفر";
        PriceValueText = "غير متوفر";
        AboutText = "تعذر تحميل بيانات الصالة.";
        MapImageUrl = string.Empty;

        GalleryImages.Clear();
        GalleryStatusText = "لا توجد صور متاحة لهذه الصالة.";

        Services.Clear();
        ServicesStatusText = "لا توجد خدمات مسجلة لهذه الصالة.";
    }

    private void RaiseCommandStates()
    {
        BackCommand.RaiseCanExecuteChanged();
        ToggleFavoriteCommand.RaiseCanExecuteChanged();
        BookNowCommand.RaiseCanExecuteChanged();
    }

    private static string BuildCapacityFromCard(string capacityText)
    {
        var digits = new string((capacityText ?? string.Empty).Where(char.IsDigit).ToArray());

        if (!int.TryParse(digits, out var capacity) || capacity <= 0)
        {
            return "غير متوفر";
        }

        return $"{capacity:N0} شخص";
    }

    private static string BuildPriceText(HallReadResponse? hall, string fallbackPriceValueText)
    {
        if (hall is not null)
        {
            if (hall.PricePerDay > 0)
            {
                return $"{hall.PricePerDay:N0} ر.س";
            }

            if (hall.PricePerHour > 0)
            {
                return $"{hall.PricePerHour:N0} ر.س / ساعة";
            }
        }

        return string.IsNullOrWhiteSpace(fallbackPriceValueText)
            ? "غير متوفر"
            : fallbackPriceValueText;
    }

    private static string BuildMapImageUrl(double latitude, double longitude)
    {
        if (Math.Abs(latitude) < 0.000001 || Math.Abs(longitude) < 0.000001)
        {
            return string.Empty;
        }

        var lat = latitude.ToString("0.######", CultureInfo.InvariantCulture);
        var lon = longitude.ToString("0.######", CultureInfo.InvariantCulture);
        return $"https://staticmap.openstreetmap.de/staticmap.php?center={lat},{lon}&zoom=14&size=800x320&markers={lat},{lon},red-pushpin";
    }

    private static bool IsHttpUrl(string? value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}

