using AfrahAPI.Models.DTOs.Auth;

namespace AfrahAPI.Services.Interfaces;


public interface IAuthService
{
    /// <summary>
    /// تسجيل مستخدم جديد
    /// </summary>
    /// <param name="registerDto">بيانات التسجيل</param>
    /// <returns>نتيجة التسجيل</returns>
    Task<RegisterResponseDTO> RegisterAsync(RegisterDTO registerDto);

    /// <summary>
    /// تسجيل الدخول
    /// </summary>
    /// <param name="loginDto">بيانات تسجيل الدخول</param>
    /// <returns>نتيجة تسجيل الدخول مع JWT Token</returns>
    Task<LoginResponseDTO?> LoginAsync(LoginDTO loginDto);

    /// <summary>
    /// تغيير كلمة المرور
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <param name="changePasswordDto">بيانات تغيير كلمة المرور</param>
    /// <returns>نتيجة العملية</returns>
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDTO changePasswordDto);
}
