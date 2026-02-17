using AfrahAPI.Models.DTOs.Auth;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة حسابات المستخدمين والمصادقة
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor
    /// </summary>
    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// تسجيل مستخدم جديد
    /// </summary>
    /// <param name="registerDto">بيانات التسجيل</param>
    /// <returns>نتيجة عملية التسجيل</returns>
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register([FromBody] RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(registerDto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// تسجيل الدخول
    /// </summary>
    /// <param name="loginDto">بيانات تسجيل الدخول</param>
    /// <returns>JWT Token ومعلومات المستخدم</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
            return Unauthorized(new { message = "البريد الإلكتروني أو كلمة المرور غير صحيحة" });

        return Ok(result);
    }

    /// <summary>
    /// تغيير كلمة المرور (يتطلب تسجيل الدخول)
    /// </summary>
    /// <param name="changePasswordDto">بيانات تغيير كلمة المرور</param>
    /// <returns>نتيجة العملية</returns>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // الحصول على معرف المستخدم من Token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);

        if (!result)
            return BadRequest(new { message = "فشل تغيير كلمة المرور. تأكد من صحة كلمة المرور الحالية" });

        return Ok(new { message = "تم تغيير كلمة المرور بنجاح" });
    }

    /// <summary>
    /// الحصول على الأدوار المتاحة
    /// </summary>
    /// <returns>قائمة بالأدوار</returns>
    [HttpGet("roles")]
    public ActionResult<List<string>> GetAvailableRoles()
    {
        var roles = new List<string>
        {
            "Customer",
            "HallOwner",
            "Employee",
            "Admin"
        };

        return Ok(roles);
    }
}

