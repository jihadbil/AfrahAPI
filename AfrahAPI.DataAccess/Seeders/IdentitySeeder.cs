using AfrahAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Seeders;

/// <summary>
/// مُنشئ بيانات المستخدمين والأدوار
/// </summary>
public class IdentitySeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public IdentitySeeder(
        ApplicationDbContext context,
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// إنشاء بيانات المستخدمين
    /// </summary>
    public async Task SeedAsync()
    {
        // 1. إنشاء الأدوار
        await SeedRolesAsync();

        // 2. إنشاء المستخدمين
        await SeedUsersAsync();

        // 3. إنشاء العملاء
        await SeedCustomersAsync();

        // 4. إنشاء ملاك الصالات
        await SeedHallOwnersAsync();

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// إنشاء الأدوار
    /// </summary>
    private async Task SeedRolesAsync()
    {
        string[] roles = { "Admin", "Customer", "HallOwner", "Employee" };

        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
            }
        }
    }

    /// <summary>
    /// إنشاء مستخدمين تجريبيين
    /// </summary>
    private async Task SeedUsersAsync()
    {
        // مستخدم Admin
        if (await _userManager.FindByEmailAsync("admin@afrahapi.com") == null)
        {
            var adminUser = new IdentityUser<Guid>
            {
                Id = Guid.NewGuid(),
                UserName = "admin@afrahapi.com",
                Email = "admin@afrahapi.com",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(adminUser, "Admin@123");
            await _userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    /// <summary>
    /// إنشاء عملاء تجريبيين
    /// </summary>
    private async Task SeedCustomersAsync()
    {
        if (await _context.Customers.AnyAsync())
            return;

        var customers = new List<(IdentityUser<Guid> User, Customer Customer)>();

        // عميل 1
        var user1 = new IdentityUser<Guid>
        {
            Id = Guid.NewGuid(),
            UserName = "ahmed.salem@gmail.com",
            Email = "ahmed.salem@gmail.com",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(user1, "Customer@123");
        await _userManager.AddToRoleAsync(user1, "Customer");

        var customer1 = new Customer
        {
            CustomerID = Guid.NewGuid(),
            UserID = user1.Id,
            FirstName = "أحمد",
            LastName = "سالم",
            Gender = "ذكر",
            Country = "السعودية",
            City = "الرياض",
            CreatedAt = DateTime.UtcNow
        };

        // عميل 2
        var user2 = new IdentityUser<Guid>
        {
            Id = Guid.NewGuid(),
            UserName = "fatima.ali@gmail.com",
            Email = "fatima.ali@gmail.com",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(user2, "Customer@123");
        await _userManager.AddToRoleAsync(user2, "Customer");

        var customer2 = new Customer
        {
            CustomerID = Guid.NewGuid(),
            UserID = user2.Id,
            FirstName = "فاطمة",
            LastName = "علي",
            Gender = "أنثى",
            Country = "السعودية",
            City = "جدة",
            CreatedAt = DateTime.UtcNow
        };

        // عميل 3
        var user3 = new IdentityUser<Guid>
        {
            Id = Guid.NewGuid(),
            UserName = "mohammed.hassan@gmail.com",
            Email = "mohammed.hassan@gmail.com",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(user3, "Customer@123");
        await _userManager.AddToRoleAsync(user3, "Customer");

        var customer3 = new Customer
        {
            CustomerID = Guid.NewGuid(),
            UserID = user3.Id,
            FirstName = "محمد",
            LastName = "حسن",
            Gender = "ذكر",
            Country = "السعودية",
            City = "الدمام",
            CreatedAt = DateTime.UtcNow
        };

        await _context.Customers.AddRangeAsync(new[] { customer1, customer2, customer3 });
    }

    /// <summary>
    /// إنشاء ملاك صالات تجريبيين
    /// </summary>
    private async Task SeedHallOwnersAsync()
    {
        if (await _context.HallOwners.AnyAsync())
            return;

        // مالك 1
        var ownerUser1 = new IdentityUser<Guid>
        {
            Id = Guid.NewGuid(),
            UserName = "owner1@afrahapi.com",
            Email = "owner1@afrahapi.com",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(ownerUser1, "Owner@123");
        await _userManager.AddToRoleAsync(ownerUser1, "HallOwner");

        var owner1 = new HallOwner
        {
            OwnerID = Guid.NewGuid(),
            UserID = ownerUser1.Id,
            FirstName = "عبدالله",
            LastName = "المطيري",
            Email = "owner1@afrahapi.com",
            Country = "السعودية",
            City = "الرياض",
            CreatedAt = DateTime.UtcNow
        };

        // مالك 2
        var ownerUser2 = new IdentityUser<Guid>
        {
            Id = Guid.NewGuid(),
            UserName = "owner2@afrahapi.com",
            Email = "owner2@afrahapi.com",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(ownerUser2, "Owner@123");
        await _userManager.AddToRoleAsync(ownerUser2, "HallOwner");

        var owner2 = new HallOwner
        {
            OwnerID = Guid.NewGuid(),
            UserID = ownerUser2.Id,
            FirstName = "سارة",
            LastName = "القحطاني",
            Email = "owner2@afrahapi.com",
            Country = "السعودية",
            City = "جدة",
            CreatedAt = DateTime.UtcNow
        };

        await _context.HallOwners.AddRangeAsync(new[] { owner1, owner2 });
    }
}
