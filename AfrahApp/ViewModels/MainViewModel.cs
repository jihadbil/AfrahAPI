using System.Collections.ObjectModel;
using System.Globalization;
using AfrahApp.Infrastructure;
using AfrahApp.Models.Booking;
using AfrahApp.Models.Hall;
using AfrahApp.Models.Home;
using AfrahApp.Services;
using Microsoft.Maui.Controls;

namespace AfrahApp.ViewModels;

public sealed class MainViewModel : BaseViewModel
{
    private const string DefaultProfileAvatarUrl =
        "https://lh3.googleusercontent.com/aida-public/AB6AXuBbsYIycDNpzfzEqNk-oPF5HzB5BY6LSmiYUnSKJ7fgi1hQsZUZHHDw8gY5MeS-mPJ_PRQQkEZwgfC8EJbxJQK0Ffs7GBw8S9AggYSr1Ua_7FzhYJU_qXdUVIzyVgqVuz35KmuCDIb9j-G2KexT1oC4_1Em1okD0hMiPs_hRbZxQrKeeXIWZS8-OYmT9EJwBdGsOtkxM8Mvmo-rr0qj2B1NTuuVLmNrZ5tc0S3XFMVq8CbpnRhqhhmP7FfTdDeNpKOVGhOK_zvxVLU";
    private static readonly CultureInfo ArabicCulture = new("ar-EG");

    private readonly AuthSession _authSession;
    private readonly IHallBookingApiClient _hallBookingApiClient;
    private readonly IAppNavigator _appNavigator;
    private readonly IAlertService _alertService;
    private readonly List<HomeHallCard> _hallBrowseSource = [];
    private readonly List<BookingCard> _bookingCardsSource = [];

    private string _welcomeText = "مرحبًا بك";
    private string _userName = "-";
    private string _email = "-";
    private string _accountStatus = "غير متصل";
    private string _sessionTokenPreview = "لا يوجد رمز دخول محفوظ.";
    private string _hallSearchQuery = string.Empty;
    private string _hallsStatusMessage = string.Empty;
    private string _reservationsStatusMessage = string.Empty;
    private string? _hallsLoadFailureMessage;
    private int _activeBookingsCount;
    private int _pendingBookingsCount;
    private int _completedBookingsCount;
    private HomeHallCard? _featuredHall;
    private ReservationsFilter _activeReservationsFilter = ReservationsFilter.All;
    private MainSection _activeSection = MainSection.Home;

    public MainViewModel(
        AuthSession authSession,
        IHallBookingApiClient hallBookingApiClient,
        IAppNavigator appNavigator,
        IAlertService alertService)
    {
        _authSession = authSession;
        _hallBookingApiClient = hallBookingApiClient;
        _appNavigator = appNavigator;
        _alertService = alertService;

        AvailableHalls = [];
        Reservations = [];
        BookingCards = [];
        RecommendedHalls = [];
        HighlightedHalls = [];
        HallsGridLeft = [];
        HallsGridRight = [];

        LogoutCommand = new AsyncCommand(LogoutAsync, () => IsNotBusy);
        ShowHomeCommand = new AsyncCommand(ShowHomeAsync, () => IsNotBusy);
        ShowHallsCommand = new AsyncCommand(ShowHallsAsync, () => IsNotBusy);
        ShowReservationsCommand = new AsyncCommand(ShowReservationsAsync, () => IsNotBusy);
        ShowProfileCommand = new AsyncCommand(ShowProfileAsync, () => IsNotBusy);
        HeaderActionCommand = new AsyncCommand(HeaderActionAsync, () => IsNotBusy);
        OpenHallDetailsCommand = new Command<HomeHallCard>(
            async hallCard => await OpenHallDetailsAsync(hallCard),
            hallCard => hallCard is not null && IsNotBusy);
        ShowAllReservationsFilterCommand = new Command(
            () => SetReservationsFilter(ReservationsFilter.All));
        ShowUpcomingReservationsFilterCommand = new Command(
            () => SetReservationsFilter(ReservationsFilter.Upcoming));
        ShowCompletedReservationsFilterCommand = new Command(
            () => SetReservationsFilter(ReservationsFilter.Completed));
        OpenReservationActionCommand = new Command<BookingCard>(
            async bookingCard => await OpenReservationActionAsync(bookingCard),
            bookingCard => bookingCard is not null && IsNotBusy);
    }

    public string WelcomeText
    {
        get => _welcomeText;
        private set => SetProperty(ref _welcomeText, value);
    }

    public string UserName
    {
        get => _userName;
        private set => SetProperty(ref _userName, value);
    }

    public string Email
    {
        get => _email;
        private set => SetProperty(ref _email, value);
    }

    public string AccountStatus
    {
        get => _accountStatus;
        private set => SetProperty(ref _accountStatus, value);
    }

    public string SessionTokenPreview
    {
        get => _sessionTokenPreview;
        private set => SetProperty(ref _sessionTokenPreview, value);
    }

    public string HallSearchQuery
    {
        get => _hallSearchQuery;
        set
        {
            if (SetProperty(ref _hallSearchQuery, value))
            {
                ApplyHallBrowseLayout();
            }
        }
    }

    public string HallsStatusMessage
    {
        get => _hallsStatusMessage;
        private set
        {
            if (SetProperty(ref _hallsStatusMessage, value))
            {
                OnPropertyChanged(nameof(IsHallsStatusVisible));
            }
        }
    }

    public string ReservationsStatusMessage
    {
        get => _reservationsStatusMessage;
        private set
        {
            if (SetProperty(ref _reservationsStatusMessage, value))
            {
                OnPropertyChanged(nameof(IsReservationsStatusVisible));
            }
        }
    }

    public int ActiveBookingsCount
    {
        get => _activeBookingsCount;
        private set => SetProperty(ref _activeBookingsCount, value);
    }

    public int PendingBookingsCount
    {
        get => _pendingBookingsCount;
        private set => SetProperty(ref _pendingBookingsCount, value);
    }

    public int CompletedBookingsCount
    {
        get => _completedBookingsCount;
        private set => SetProperty(ref _completedBookingsCount, value);
    }

    public string ProfileAvatarUrl => DefaultProfileAvatarUrl;
    public string WelcomeBackText => "مرحبًا بعودتك،";
    public string MembershipText => "عضو ذهبي";

    public string ActiveSectionTitle => _activeSection switch
    {
        MainSection.Home => "الرئيسية",
        MainSection.Halls => "الصالات المتاحة",
        MainSection.Reservations => "حجوزاتي",
        MainSection.Profile => "الملف الشخصي",
        _ => "الرئيسية"
    };

    public bool IsHomeSectionVisible => _activeSection == MainSection.Home;
    public bool IsHallsSectionVisible => _activeSection == MainSection.Halls;
    public bool IsReservationsSectionVisible => _activeSection == MainSection.Reservations;
    public bool IsProfileSectionVisible => _activeSection == MainSection.Profile;

    public string HomeTabColor => _activeSection == MainSection.Home ? "#E6195D" : "#88636F";
    public string ReservationsTabColor => _activeSection == MainSection.Reservations ? "#E6195D" : "#88636F";
    public string HallsTabColor => _activeSection == MainSection.Halls ? "#E6195D" : "#88636F";
    public string ProfileTabColor => _activeSection == MainSection.Profile ? "#E6195D" : "#88636F";
    public string HeaderActionIcon => _activeSection == MainSection.Home ? "☰" : "→";
    public bool IsHeaderBellVisible => _activeSection is MainSection.Home or MainSection.Halls;
    public bool IsHeaderFilterVisible => _activeSection == MainSection.Reservations;
    public bool IsHeaderSaveVisible => _activeSection == MainSection.Profile;

    public HomeHallCard? FeaturedHall
    {
        get => _featuredHall;
        private set
        {
            if (SetProperty(ref _featuredHall, value))
            {
                OnPropertyChanged(nameof(HasFeaturedHall));
            }
        }
    }

    public bool HasFeaturedHall => FeaturedHall is not null;
    public bool HasHallsData => FeaturedHall is not null || HighlightedHalls.Count > 0 || HallsGridLeft.Count > 0 || HallsGridRight.Count > 0;
    public bool IsHallsStatusVisible => !string.IsNullOrWhiteSpace(HallsStatusMessage);
    public bool IsReservationsStatusVisible => !string.IsNullOrWhiteSpace(ReservationsStatusMessage);
    public bool IsReservationsAllTabActive => _activeReservationsFilter == ReservationsFilter.All;
    public bool IsReservationsUpcomingTabActive => _activeReservationsFilter == ReservationsFilter.Upcoming;
    public bool IsReservationsCompletedTabActive => _activeReservationsFilter == ReservationsFilter.Completed;
    public string ReservationsAllTabColor => IsReservationsAllTabActive ? "#181113" : "#617189";
    public string ReservationsUpcomingTabColor => IsReservationsUpcomingTabActive ? "#181113" : "#617189";
    public string ReservationsCompletedTabColor => IsReservationsCompletedTabActive ? "#181113" : "#617189";

    public ObservableCollection<string> AvailableHalls { get; }
    public ObservableCollection<string> Reservations { get; }
    public ObservableCollection<BookingCard> BookingCards { get; }
    public ObservableCollection<HomeHallCard> RecommendedHalls { get; }
    public ObservableCollection<HomeHallCard> HighlightedHalls { get; }
    public ObservableCollection<HomeHallCard> HallsGridLeft { get; }
    public ObservableCollection<HomeHallCard> HallsGridRight { get; }

    public AsyncCommand LogoutCommand { get; }
    public AsyncCommand ShowHomeCommand { get; }
    public AsyncCommand ShowHallsCommand { get; }
    public AsyncCommand ShowReservationsCommand { get; }
    public AsyncCommand ShowProfileCommand { get; }
    public AsyncCommand HeaderActionCommand { get; }
    public Command<HomeHallCard> OpenHallDetailsCommand { get; }
    public Command ShowAllReservationsFilterCommand { get; }
    public Command ShowUpcomingReservationsFilterCommand { get; }
    public Command ShowCompletedReservationsFilterCommand { get; }
    public Command<BookingCard> OpenReservationActionCommand { get; }

    public async Task LoadAsync()
    {
        IsBusy = true;
        RaiseCommandStates();

        try
        {
            var token = await _authSession.GetTokenAsync();
            var userName = _authSession.GetUserName();
            var email = _authSession.GetEmail();
            var userId = _authSession.GetUserId();

            UserName = string.IsNullOrWhiteSpace(userName) ? "مستخدم" : userName;
            Email = string.IsNullOrWhiteSpace(email) ? "-" : email;
            WelcomeText = $"مرحبًا، {UserName}";

            var tokenValue = token ?? string.Empty;
            var hasToken = !string.IsNullOrWhiteSpace(tokenValue);
            AccountStatus = hasToken ? "تم تسجيل الدخول" : "غير متصل";
            SessionTokenPreview = hasToken
                ? $"Token: {tokenValue[..Math.Min(tokenValue.Length, 120)]}..."
                : "لا يوجد رمز دخول محفوظ.";

            var hallsById = await LoadHallsAsync();
            await LoadBookingsAsync(userId, hallsById);
        }
        finally
        {
            IsBusy = false;
            RaiseCommandStates();
        }
    }

    private async Task<Dictionary<Guid, HallReadResponse>> LoadHallsAsync()
    {
        var hallsResult = await _hallBookingApiClient.GetHallsAsync();
        var hallsById = new Dictionary<Guid, HallReadResponse>();

        AvailableHalls.Clear();
        RecommendedHalls.Clear();
        _hallBrowseSource.Clear();
        _hallsLoadFailureMessage = null;

        if (!hallsResult.IsSuccess)
        {
            _hallsLoadFailureMessage = $"تعذر تحميل الصالات: {hallsResult.Message}";
            AvailableHalls.Add(_hallsLoadFailureMessage);
            ApplyHallBrowseLayout();
            return hallsById;
        }

        if (hallsResult.Data is null || hallsResult.Data.Count == 0)
        {
            _hallsLoadFailureMessage = "لا توجد صالات متاحة حاليًا.";
            AvailableHalls.Add(_hallsLoadFailureMessage);
            ApplyHallBrowseLayout();
            return hallsById;
        }

        foreach (var hall in hallsResult.Data)
        {
            hallsById[hall.HallID] = hall;
        }

        var orderedHalls = hallsResult.Data
            .OrderByDescending(h => h.IsAvailable)
            .ThenBy(h => h.HallName)
            .ToList();

        for (var i = 0; i < orderedHalls.Count; i++)
        {
            var hall = orderedHalls[i];

            AvailableHalls.Add(FormatHall(hall));

            var hallCard = CreateHallCard(hall);
            _hallBrowseSource.Add(hallCard);

            if (RecommendedHalls.Count < 10)
            {
                RecommendedHalls.Add(hallCard);
            }
        }

        ApplyHallBrowseLayout();
        return hallsById;
    }

    private async Task LoadBookingsAsync(Guid userId, IReadOnlyDictionary<Guid, HallReadResponse> hallsById)
    {
        var bookingsResult = await _hallBookingApiClient.GetBookingsByUserAsync(userId);

        Reservations.Clear();
        BookingCards.Clear();
        _bookingCardsSource.Clear();
        ReservationsStatusMessage = string.Empty;
        ActiveBookingsCount = 0;
        PendingBookingsCount = 0;
        CompletedBookingsCount = 0;

        if (!bookingsResult.IsSuccess)
        {
            ReservationsStatusMessage = $"تعذر تحميل الحجوزات: {bookingsResult.Message}";
            return;
        }

        if (bookingsResult.Data is null || bookingsResult.Data.Count == 0)
        {
            ReservationsStatusMessage = "لا توجد حجوزات حالية.";
            return;
        }

        var active = 0;
        var pending = 0;
        var completed = 0;
        
        var sortedBookings = bookingsResult.Data
            .OrderBy(booking => GetBookingDisplayOrder(booking.Status))
            .ThenByDescending(booking => booking.StartDate);

        foreach (var booking in sortedBookings)
        {
            Reservations.Add(FormatBooking(booking, hallsById));
            _bookingCardsSource.Add(CreateBookingCard(booking, hallsById));

            switch (ClassifyBookingStatus(booking.Status))
            {
                case BookingStatusBucket.Completed:
                    completed++;
                    break;
                case BookingStatusBucket.Pending:
                    pending++;
                    break;
                case BookingStatusBucket.Cancelled:
                    break;
                default:
                    active++;
                    break;
            }
        }

        if (active == 0 && pending == 0 && completed == 0)
        {
            active = bookingsResult.Data.Count;
        }

        ActiveBookingsCount = active;
        PendingBookingsCount = pending;
        CompletedBookingsCount = completed;

        ApplyReservationsFilter();
    }

    private async Task LogoutAsync()
    {
        _authSession.Clear();
        await _appNavigator.GoToLoginAsync();
    }

    private Task ShowHomeAsync()
    {
        SetActiveSection(MainSection.Home);
        return Task.CompletedTask;
    }

    private Task ShowHallsAsync()
    {
        SetActiveSection(MainSection.Halls);
        return Task.CompletedTask;
    }

    private Task ShowReservationsAsync()
    {
        SetActiveSection(MainSection.Reservations);
        return Task.CompletedTask;
    }

    private Task ShowProfileAsync()
    {
        SetActiveSection(MainSection.Profile);
        return Task.CompletedTask;
    }

    private Task HeaderActionAsync()
    {
        if (_activeSection != MainSection.Home)
        {
            SetActiveSection(MainSection.Home);
        }

        return Task.CompletedTask;
    }

    private async Task OpenHallDetailsAsync(HomeHallCard? hallCard)
    {
        if (hallCard is null || IsBusy)
        {
            return;
        }

        await _appNavigator.GoToHallDetailsAsync(hallCard);
    }

    private async Task OpenReservationActionAsync(BookingCard? bookingCard)
    {
        if (bookingCard is null || IsBusy)
        {
            return;
        }

        if (bookingCard.IsCancelled)
        {
            await _alertService.ShowAsync("سبب الإلغاء", "تم إلغاء هذا الحجز بسبب انتهاء المهلة أو تعذر تأكيد الدفع.");
            return;
        }

        var hallCard = _hallBrowseSource.FirstOrDefault(card => card.HallId == bookingCard.HallId)
            ?? new HomeHallCard
            {
                HallId = bookingCard.HallId,
                Name = bookingCard.HallName,
                Location = bookingCard.HallLocation,
                ImageUrl = bookingCard.HallImageUrl,
                PriceValueText = bookingCard.PriceText,
                PriceText = $"تبدأ من {bookingCard.PriceText}",
                RatingText = string.Empty,
                Description = string.Empty,
                ReviewsCount = 0,
                IsAvailable = !bookingCard.IsCancelled
            };

        await _appNavigator.GoToHallDetailsAsync(hallCard);
    }

    private void SetReservationsFilter(ReservationsFilter filter)
    {
        if (_activeReservationsFilter == filter && BookingCards.Count > 0)
        {
            return;
        }

        _activeReservationsFilter = filter;
        ApplyReservationsFilter();
        OnPropertyChanged(nameof(IsReservationsAllTabActive));
        OnPropertyChanged(nameof(IsReservationsUpcomingTabActive));
        OnPropertyChanged(nameof(IsReservationsCompletedTabActive));
        OnPropertyChanged(nameof(ReservationsAllTabColor));
        OnPropertyChanged(nameof(ReservationsUpcomingTabColor));
        OnPropertyChanged(nameof(ReservationsCompletedTabColor));
    }

    private void ApplyReservationsFilter()
    {
        BookingCards.Clear();

        IEnumerable<BookingCard> cards = _bookingCardsSource;

        cards = _activeReservationsFilter switch
        {
            ReservationsFilter.Upcoming => cards.Where(card =>
                card.StatusBucket is nameof(BookingStatusBucket.Active) or nameof(BookingStatusBucket.Pending)),
            ReservationsFilter.Completed => cards.Where(card =>
                card.StatusBucket is nameof(BookingStatusBucket.Completed) or nameof(BookingStatusBucket.Cancelled)),
            _ => cards
        };

        foreach (var card in cards)
        {
            BookingCards.Add(card);
        }

        ReservationsStatusMessage = BookingCards.Count == 0
            ? _activeReservationsFilter switch
            {
                ReservationsFilter.Upcoming => "لا توجد حجوزات قادمة.",
                ReservationsFilter.Completed => "لا توجد حجوزات مكتملة.",
                _ => "لا توجد حجوزات حالية."
            }
            : string.Empty;
    }

    private void SetActiveSection(MainSection section)
    {
        if (_activeSection == section)
        {
            return;
        }

        _activeSection = section;
        OnPropertyChanged(nameof(ActiveSectionTitle));
        OnPropertyChanged(nameof(IsHomeSectionVisible));
        OnPropertyChanged(nameof(IsHallsSectionVisible));
        OnPropertyChanged(nameof(IsReservationsSectionVisible));
        OnPropertyChanged(nameof(IsProfileSectionVisible));
        OnPropertyChanged(nameof(HomeTabColor));
        OnPropertyChanged(nameof(ReservationsTabColor));
        OnPropertyChanged(nameof(HallsTabColor));
        OnPropertyChanged(nameof(ProfileTabColor));
        OnPropertyChanged(nameof(HeaderActionIcon));
        OnPropertyChanged(nameof(IsHeaderBellVisible));
        OnPropertyChanged(nameof(IsHeaderFilterVisible));
        OnPropertyChanged(nameof(IsHeaderSaveVisible));
    }

    private void RaiseCommandStates()
    {
        LogoutCommand.RaiseCanExecuteChanged();
        ShowHomeCommand.RaiseCanExecuteChanged();
        ShowHallsCommand.RaiseCanExecuteChanged();
        ShowReservationsCommand.RaiseCanExecuteChanged();
        ShowProfileCommand.RaiseCanExecuteChanged();
        HeaderActionCommand.RaiseCanExecuteChanged();
        OpenHallDetailsCommand.ChangeCanExecute();
        OpenReservationActionCommand.ChangeCanExecute();
    }

    private static string FormatHall(HallReadResponse hall)
    {
        var availability = hall.IsAvailable ? "متاحة" : "غير متاحة";
        var verification = hall.IsVerified ? "موثقة" : "غير موثقة";
        var address = string.IsNullOrWhiteSpace(hall.Address) ? "بدون عنوان" : hall.Address;

        return $"{hall.HallName} | {address} | السعة: {hall.Capacity} | اليوم: {hall.PricePerDay:0.##} | {availability} | {verification}";
    }

    private static string FormatBooking(
        BookingReadResponse booking,
        IReadOnlyDictionary<Guid, HallReadResponse> hallsById)
    {
        var hallName = hallsById.TryGetValue(booking.HallId, out var value)
            ? value.HallName
            : $"Hall #{booking.HallId.ToString("N")[..8]}";

        return $"حجز #{booking.BookingId.ToString("N")[..8]} | {hallName} | {booking.StartDate:yyyy-MM-dd} - {booking.EndDate:yyyy-MM-dd} | الحالة: {booking.Status} | الإجمالي: {booking.TotalPrice:0.##}";
    }

    private static HomeHallCard CreateHallCard(HallReadResponse hall)
    {
        var imageUrl = string.IsNullOrWhiteSpace(hall.MainImageUrl)
            ? string.Empty
            : hall.MainImageUrl;

        var location = string.IsNullOrWhiteSpace(hall.Address) ? "المملكة العربية السعودية" : hall.Address;
        var priceValueText = $"{hall.PricePerDay:N0} ر.س";
        var description = string.IsNullOrWhiteSpace(hall.Description)
            ? "لا يوجد وصف متاح لهذه الصالة."
            : hall.Description!;

        return new HomeHallCard
        {
            HallId = hall.HallID,
            Name = hall.HallName,
            Location = location,
            ImageUrl = imageUrl ?? string.Empty,
            RatingText = "—",
            PriceText = $"تبدأ من {priceValueText}",
            PriceValueText = priceValueText,
            CapacityText = hall.Capacity > 0 ? $"سعة {hall.Capacity:N0} شخص" : "السعة غير متاحة",
            TagText = IsOutdoorVenue(hall.HallName, location) ? "خارجي" : string.Empty,
            Description = description,
            ReviewsCount = 0,
            IsAvailable = hall.IsAvailable
        };
    }

    private BookingCard CreateBookingCard(
        BookingReadResponse booking,
        IReadOnlyDictionary<Guid, HallReadResponse> hallsById)
    {
        var hallName = hallsById.TryGetValue(booking.HallId, out var hall)
            ? hall.HallName
            : $"قاعة #{booking.HallId.ToString("N")[..6]}";

        var hallLocation = hallsById.TryGetValue(booking.HallId, out hall)
            ? (string.IsNullOrWhiteSpace(hall.Address) ? "المملكة العربية السعودية" : hall.Address!)
            : "المملكة العربية السعودية";

        var hallImageUrl = hallsById.TryGetValue(booking.HallId, out hall) && !string.IsNullOrWhiteSpace(hall.MainImageUrl)
            ? hall.MainImageUrl!
            : string.Empty;

        var statusBucket = ClassifyBookingStatus(booking.Status);
        var isCancelled = statusBucket == BookingStatusBucket.Cancelled;
        var isCompleted = statusBucket == BookingStatusBucket.Completed;

        var statusText = statusBucket switch
        {
            BookingStatusBucket.Pending => "قيد الانتظار",
            BookingStatusBucket.Completed => "مكتملة",
            BookingStatusBucket.Cancelled => "ملغي",
            _ => "مؤكد"
        };

        var statusIcon = statusBucket switch
        {
            BookingStatusBucket.Pending => "◔",
            BookingStatusBucket.Completed => "◌",
            BookingStatusBucket.Cancelled => "✕",
            _ => "✓"
        };

        var statusBackgroundColor = statusBucket switch
        {
            BookingStatusBucket.Pending => "#FDF0CF",
            BookingStatusBucket.Completed => "#EEF2F6",
            BookingStatusBucket.Cancelled => "#FDEBEC",
            _ => "#DDF5E6"
        };

        var statusTextColor = statusBucket switch
        {
            BookingStatusBucket.Pending => "#AD6400",
            BookingStatusBucket.Completed => "#617189",
            BookingStatusBucket.Cancelled => "#D14343",
            _ => "#167A49"
        };

        var actionText = statusBucket switch
        {
            BookingStatusBucket.Completed => "إعادة الحجز",
            BookingStatusBucket.Cancelled => "عرض السبب",
            _ => "التفاصيل"
        };

        var actionIcon = statusBucket switch
        {
            BookingStatusBucket.Completed => "↻",
            BookingStatusBucket.Cancelled => "!",
            _ => "‹"
        };

        var dateText = booking.StartDate.ToString("d MMMM yyyy", ArabicCulture);
        var timeText = FormatArabicTime(booking.StartDate);

        return new BookingCard
        {
            BookingId = booking.BookingId,
            HallId = booking.HallId,
            StartDate = booking.StartDate,
            StatusBucket = statusBucket.ToString(),
            HallName = hallName,
            HallLocation = hallLocation,
            HallImageUrl = hallImageUrl,
            StatusText = statusText,
            StatusIcon = statusIcon,
            StatusBackgroundColor = statusBackgroundColor,
            StatusTextColor = statusTextColor,
            DateText = dateText,
            TimeText = timeText,
            ShowTime = !isCancelled,
            PriceText = $"{booking.TotalPrice:N0} ر.س",
            PriceTextColor = isCompleted ? "#93A1B6" : "#E6195D",
            ShowPrice = !isCancelled,
            ActionText = actionText,
            ActionIcon = actionIcon,
            ActionTextColor = "#617189",
            IsCancelled = isCancelled,
            IsMuted = isCompleted || isCancelled
        };
    }

    private static int GetBookingDisplayOrder(string? status)
    {
        return ClassifyBookingStatus(status) switch
        {
            BookingStatusBucket.Active => 0,
            BookingStatusBucket.Pending => 1,
            BookingStatusBucket.Completed => 2,
            BookingStatusBucket.Cancelled => 3,
            _ => 4
        };
    }

    private static string FormatArabicTime(DateTime value)
    {
        var suffix = value.Hour >= 12 ? "مساءً" : "صباحًا";
        return $"{value:h:mm} {suffix}";
    }

    private void ApplyHallBrowseLayout()
    {
        FeaturedHall = null;
        HighlightedHalls.Clear();
        HallsGridLeft.Clear();
        HallsGridRight.Clear();

        if (!string.IsNullOrWhiteSpace(_hallsLoadFailureMessage))
        {
            HallsStatusMessage = _hallsLoadFailureMessage;
            OnPropertyChanged(nameof(HasHallsData));
            return;
        }

        IEnumerable<HomeHallCard> filteredCards = _hallBrowseSource;
        var query = HallSearchQuery.Trim();

        if (!string.IsNullOrWhiteSpace(query))
        {
            filteredCards = filteredCards.Where(card => MatchesHallSearch(card, query));
        }

        var cards = filteredCards.ToList();

        if (cards.Count == 0)
        {
            HallsStatusMessage = _hallBrowseSource.Count == 0
                ? "لا توجد صالات متاحة حاليًا."
                : "لا توجد نتائج مطابقة للبحث.";
            OnPropertyChanged(nameof(HasHallsData));
            return;
        }

        if (cards.Count == 1)
        {
            FeaturedHall = cards[0];
            HallsStatusMessage = string.Empty;
            OnPropertyChanged(nameof(HasHallsData));
            return;
        }

        for (var i = 0; i < cards.Count; i++)
        {
            if (i % 2 == 0)
            {
                HallsGridLeft.Add(cards[i]);
            }
            else
            {
                HallsGridRight.Add(cards[i]);
            }
        }

        HallsStatusMessage = string.Empty;
        OnPropertyChanged(nameof(HasHallsData));
    }

    private static bool MatchesHallSearch(HomeHallCard hallCard, string query)
    {
        return hallCard.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
               hallCard.Location.Contains(query, StringComparison.OrdinalIgnoreCase) ||
               hallCard.CapacityText.Contains(query, StringComparison.OrdinalIgnoreCase) ||
               hallCard.PriceValueText.Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsOutdoorVenue(string hallName, string location)
    {
        var searchText = $"{hallName} {location}".ToLowerInvariant();
        return searchText.Contains("garden") ||
               searchText.Contains("outdoor") ||
               searchText.Contains("حديقة") ||
               searchText.Contains("خارجي");
    }

    private static BookingStatusBucket ClassifyBookingStatus(string? status)
    {
        var value = status?.Trim().ToLowerInvariant() ?? string.Empty;

        if (value.Contains("cancel") || value.Contains("decline") || value.Contains("reject") ||
            value.Contains("ملغ") || value.Contains("مرفوض"))
        {
            return BookingStatusBucket.Cancelled;
        }

        if (value.Contains("complete") || value.Contains("done") || value.Contains("finished") || value.Contains("مكتمل"))
        {
            return BookingStatusBucket.Completed;
        }

        if (value.Contains("pending") || value.Contains("wait") || value.Contains("await") || value.Contains("draft") ||
            value.Contains("انتظار") || value.Contains("قيد") || value.Contains("معلق"))
        {
            return BookingStatusBucket.Pending;
        }

        return BookingStatusBucket.Active;
    }

    private enum MainSection
    {
        Home,
        Halls,
        Reservations,
        Profile
    }

    private enum BookingStatusBucket
    {
        Active,
        Pending,
        Completed,
        Cancelled
    }

    private enum ReservationsFilter
    {
        All,
        Upcoming,
        Completed
    }
}

