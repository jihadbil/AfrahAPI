using AfrahApp.Models.Auth;
using AfrahApp.Models.Common;

namespace AfrahApp.Services;

public interface IAuthApiClient
{
    Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<ApiResult<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}
