using AfrahApp.Models.Booking;
using AfrahApp.Models.Common;
using AfrahApp.Models.Hall;

namespace AfrahApp.Services;

public interface IHallBookingApiClient
{
    Task<ApiResult<IReadOnlyList<HallReadResponse>>> GetHallsAsync(
        CancellationToken cancellationToken = default);

    Task<ApiResult<HallReadResponse>> GetHallByIdAsync(
        Guid hallId,
        CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<HallRatingReadResponse>>> GetHallRatingsAsync(
        Guid hallId,
        CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<HallServiceReadResponse>>> GetHallServicesAsync(
        Guid hallId,
        CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<HallMediaReadResponse>>> GetHallMediaAsync(
        Guid hallId,
        CancellationToken cancellationToken = default);

    Task<ApiResult<IReadOnlyList<BookingReadResponse>>> GetBookingsByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
