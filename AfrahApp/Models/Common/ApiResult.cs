namespace AfrahApp.Models.Common;

public sealed record ApiResult<T>(bool IsSuccess, string Message, T? Data)
{
    public static ApiResult<T> Success(T data, string message = "Success.")
    {
        return new ApiResult<T>(true, message, data);
    }

    public static ApiResult<T> Failure(string message, T? data = default)
    {
        return new ApiResult<T>(false, message, data);
    }
}
