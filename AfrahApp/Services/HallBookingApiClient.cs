using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AfrahApp.Models.Booking;
using AfrahApp.Models.Common;
using AfrahApp.Models.Customer;
using AfrahApp.Models.Hall;

namespace AfrahApp.Services;

public sealed class HallBookingApiClient(
    HttpClient httpClient,
    AuthSession authSession) : IHallBookingApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ApiResult<IReadOnlyList<HallReadResponse>>> GetHallsAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await SendGetAsync<List<HallReadResponse>>("Hall", cancellationToken);
        if (!result.IsSuccess)
        {
            return ApiResult<IReadOnlyList<HallReadResponse>>.Failure(result.Message);
        }

        return ApiResult<IReadOnlyList<HallReadResponse>>.Success(result.Data ?? []);
    }

    public async Task<ApiResult<HallReadResponse>> GetHallByIdAsync(
        Guid hallId,
        CancellationToken cancellationToken = default)
    {
        if (hallId == Guid.Empty)
        {
            return ApiResult<HallReadResponse>.Failure("Invalid hall identifier.");
        }

        return await SendGetAsync<HallReadResponse>($"Hall/{hallId}", cancellationToken);
    }

    public async Task<ApiResult<IReadOnlyList<HallRatingReadResponse>>> GetHallRatingsAsync(
        Guid hallId,
        CancellationToken cancellationToken = default)
    {
        if (hallId == Guid.Empty)
        {
            return ApiResult<IReadOnlyList<HallRatingReadResponse>>.Failure("Invalid hall identifier.");
        }

        var result = await SendGetAsync<List<HallRatingReadResponse>>($"HallRating/hall/{hallId}", cancellationToken);
        if (!result.IsSuccess)
        {
            return ApiResult<IReadOnlyList<HallRatingReadResponse>>.Failure(result.Message);
        }

        return ApiResult<IReadOnlyList<HallRatingReadResponse>>.Success(result.Data ?? []);
    }

    public async Task<ApiResult<IReadOnlyList<HallServiceReadResponse>>> GetHallServicesAsync(
        Guid hallId,
        CancellationToken cancellationToken = default)
    {
        if (hallId == Guid.Empty)
        {
            return ApiResult<IReadOnlyList<HallServiceReadResponse>>.Failure("Invalid hall identifier.");
        }

        var result = await SendGetAsync<List<HallServiceReadResponse>>($"HallServices/hall/{hallId}", cancellationToken);
        if (!result.IsSuccess)
        {
            return ApiResult<IReadOnlyList<HallServiceReadResponse>>.Failure(result.Message);
        }

        return ApiResult<IReadOnlyList<HallServiceReadResponse>>.Success(result.Data ?? []);
    }

    public async Task<ApiResult<IReadOnlyList<HallMediaReadResponse>>> GetHallMediaAsync(
        Guid hallId,
        CancellationToken cancellationToken = default)
    {
        if (hallId == Guid.Empty)
        {
            return ApiResult<IReadOnlyList<HallMediaReadResponse>>.Failure("Invalid hall identifier.");
        }

        var result = await SendGetAsync<List<HallMediaReadResponse>>($"HallMedia/hall/{hallId}", cancellationToken);
        if (!result.IsSuccess)
        {
            return ApiResult<IReadOnlyList<HallMediaReadResponse>>.Failure(result.Message);
        }

        return ApiResult<IReadOnlyList<HallMediaReadResponse>>.Success(result.Data ?? []);
    }

    public async Task<ApiResult<IReadOnlyList<BookingReadResponse>>> GetBookingsByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return await GetAllBookingsAsync(cancellationToken);
        }

        var customerResult = await SendGetAsync<CustomerReadResponse>(
            $"Customer/user/{userId}",
            cancellationToken);

        if (!customerResult.IsSuccess || customerResult.Data is null)
        {
            // Fallback to all bookings if the customer profile is unavailable.
            return await GetAllBookingsAsync(cancellationToken);
        }

        var bookingsResult = await SendGetAsync<List<BookingReadResponse>>(
            $"Booking/customer/{customerResult.Data.CustomerID}",
            cancellationToken);

        if (!bookingsResult.IsSuccess)
        {
            return ApiResult<IReadOnlyList<BookingReadResponse>>.Failure(bookingsResult.Message);
        }

        return ApiResult<IReadOnlyList<BookingReadResponse>>.Success(bookingsResult.Data ?? []);
    }

    private async Task<ApiResult<IReadOnlyList<BookingReadResponse>>> GetAllBookingsAsync(
        CancellationToken cancellationToken)
    {
        var result = await SendGetAsync<List<BookingReadResponse>>("Booking", cancellationToken);
        if (!result.IsSuccess)
        {
            return ApiResult<IReadOnlyList<BookingReadResponse>>.Failure(result.Message);
        }

        return ApiResult<IReadOnlyList<BookingReadResponse>>.Success(result.Data ?? []);
    }

    private async Task<ApiResult<T>> SendGetAsync<T>(
        string relativeUri,
        CancellationToken cancellationToken)
    {
        try
        {
            using var request = await CreateAuthorizedGetRequestAsync(relativeUri);
            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ReadErrorMessageAsync(response, cancellationToken);
                return ApiResult<T>.Failure(
                    $"{(int)response.StatusCode} {response.ReasonPhrase}: {errorMessage}");
            }

            var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
            if (data is null)
            {
                return ApiResult<T>.Failure("Empty response from server.");
            }

            return ApiResult<T>.Success(data);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure($"Request error: {ex.Message}");
        }
    }

    private async Task<HttpRequestMessage> CreateAuthorizedGetRequestAsync(string relativeUri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, relativeUri);
        var token = await authSession.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return request;
    }

    private static async Task<string> ReadErrorMessageAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(rawContent))
        {
            return "No response details.";
        }

        try
        {
            using var doc = JsonDocument.Parse(rawContent);
            var root = doc.RootElement;

            if (root.TryGetProperty("message", out var messageElement))
            {
                return messageElement.GetString() ?? rawContent;
            }

            if (root.TryGetProperty("title", out var titleElement))
            {
                return titleElement.GetString() ?? rawContent;
            }
        }
        catch
        {
            // Fallback to raw response body.
        }

        return rawContent;
    }
}
