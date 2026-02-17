using AfrahAPI.Models.DTOs.Auth;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تنفيذ خدمة المصادقة
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor
    /// </summary>
    public AuthService(
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    /// <summary>
    /// تسجيل مستخدم جديد
    /// </summary>
    public async Task<RegisterResponseDTO> RegisterAsync(RegisterDTO registerDto)
    {
        try
        {
            // التحقق من وجود المستخدم
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = "البريد الإلكتروني مستخدم بالفعل",
                    Errors = new List<string> { "البريد الإلكتروني مستخدم بالفعل" }
                };
            }

            // التحقق من اسم المستخدم
            var existingUserName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserName != null)
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = "اسم المستخدم مستخدم بالفعل",
                    Errors = new List<string> { "اسم المستخدم مستخدم بالفعل" }
                };
            }

            // إنشاء المستخدم
            var user = new IdentityUser<Guid>
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                EmailConfirmed = true // للتبسيط في الوقت الحالي
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponseDTO
                {
                    Success = false,
                    Message = "فشل إنشاء المستخدم",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // إضافة الدور
            await EnsureRoleExistsAsync(registerDto.Role);
            await _userManager.AddToRoleAsync(user, registerDto.Role);

            return new RegisterResponseDTO
            {
                Success = true,
                Message = "تم التسجيل بنجاح",
                UserId = user.Id,
                Email = user.Email
            };
        }
        catch (Exception ex)
        {
            return new RegisterResponseDTO
            {
                Success = false,
                Message = "حدث خطأ أثناء التسجيل",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    /// <summary>
    /// تسجيل الدخول
    /// </summary>
    public async Task<LoginResponseDTO?> LoginAsync(LoginDTO loginDto)
    {
        try
        {
            // البحث عن المستخدم بالبريد الإلكتروني أو اسم المستخدم
            var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName) 
                       ?? await _userManager.FindByNameAsync(loginDto.EmailOrUserName);

            if (user == null)
                return null;

            // التحقق من كلمة المرور
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return null;

            // التحقق من عدم قفل الحساب
            if (await _userManager.IsLockedOutAsync(user))
                return null;

            // الحصول على الأدوار
            var roles = await _userManager.GetRolesAsync(user);

            // إنشاء JWT Token
            var token = await GenerateJwtTokenAsync(user, roles.ToList());
            var refreshToken = GenerateRefreshToken();

            return new LoginResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresIn = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]) * 60,
                UserInfo = new UserInfoDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Roles = roles.ToList()
                }
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// تغيير كلمة المرور
    /// </summary>
    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDTO changePasswordDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(
                user,
                changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);

            return result.Succeeded;
        }
        catch
        {
            return false;
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// التأكد من وجود الدور وإنشائه إذا لم يكن موجوداً
    /// </summary>
    private async Task EnsureRoleExistsAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
        }
    }

    /// <summary>
    /// إنشاء JWT Token
    /// </summary>
    private async Task<string> GenerateJwtTokenAsync(IdentityUser<Guid> user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        // إضافة الأدوار كـ Claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// إنشاء Refresh Token
    /// </summary>
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    #endregion
}
