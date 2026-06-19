using System.Net.Http.Json;
using System.Text.Json;
using AfrahApp.Models.Auth;
using AfrahApp.Models.Common;
using AfrahApp.Models.Customer;

namespace AfrahApp.Services;

public sealed class AuthApiClient(HttpClient httpClient) : IAuthApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ApiResult<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Account/login", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ReadErrorMessageAsync(response, cancellationToken);
                return ApiResult<LoginResponse>.Failure($"Login failed: {errorMessage}");
            }

            var data = await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions, cancellationToken);
            if (data is null || string.IsNullOrWhiteSpace(data.Token))
            {
                return ApiResult<LoginResponse>.Failure("Login failed: empty or invalid response.");
            }

            return ApiResult<LoginResponse>.Success(data, "Login success.");
        }
        catch (Exception ex)
        {
            return ApiResult<LoginResponse>.Failure($"Login error: {ex.Message}");
        }
    }

    public async Task<ApiResult<RegisterResponse>> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var registerAccountResult = await RegisterAccountAsync(request, cancellationToken);
            if (!registerAccountResult.IsSuccess || registerAccountResult.Data is null)
            {
                return registerAccountResult;
            }

            var createCustomerResult = await CreateCustomerProfileAsync(request, registerAccountResult.Data.UserId, cancellationToken);
            if (!createCustomerResult.IsSuccess)
            {
                return ApiResult<RegisterResponse>.Failure(
                    $"User created but customer profile creation failed: {createCustomerResult.Message}",
                    registerAccountResult.Data);
            }

            return ApiResult<RegisterResponse>.Success(
                registerAccountResult.Data,
                "Customer account created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResult<RegisterResponse>.Failure($"Register error: {ex.Message}");
        }
    }

    private async Task<ApiResult<RegisterResponse>> RegisterAccountAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var payload = new AccountRegisterRequest
        {
            Email = request.Email,
            UserName = request.UserName,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            PhoneNumber = request.PhoneNumber,
            Role = "Customer"
        };

        var response = await httpClient.PostAsJsonAsync("Account/register", payload, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ReadErrorMessageAsync(response, cancellationToken);
            return ApiResult<RegisterResponse>.Failure(errorMessage);
        }

        var data = await response.Content.ReadFromJsonAsync<RegisterResponse>(JsonOptions, cancellationToken);
        if (data is null)
        {
            return ApiResult<RegisterResponse>.Failure("Register failed: empty response.");
        }

        if (!data.Success || data.UserId == Guid.Empty)
        {
            return ApiResult<RegisterResponse>.Failure(data.Message, data);
        }

        return ApiResult<RegisterResponse>.Success(data, data.Message);
    }

    private async Task<ApiResult<CustomerReadResponse>> CreateCustomerProfileAsync(
        RegisterRequest request,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var payload = new CustomerCreateRequest
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            UserID = userId
        };

        var response = await httpClient.PostAsJsonAsync("Customer", payload, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ReadErrorMessageAsync(response, cancellationToken);
            return ApiResult<CustomerReadResponse>.Failure(errorMessage);
        }

        var data = await response.Content.ReadFromJsonAsync<CustomerReadResponse>(JsonOptions, cancellationToken);
        return data is null
            ? ApiResult<CustomerReadResponse>.Failure("Customer profile creation failed: empty response.")
            : ApiResult<CustomerReadResponse>.Success(data);
    }

    private static async Task<string> ReadErrorMessageAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(rawContent))
        {
            return $"{(int)response.StatusCode} {response.ReasonPhrase}";
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

            if (root.TryGetProperty("errors", out var errorsElement) &&
                errorsElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in errorsElement.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in property.Value.EnumerateArray())
                        {
                            var value = item.GetString();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                return value;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            // Fallback to raw response body.
        }

        return rawContent;
    }
}
