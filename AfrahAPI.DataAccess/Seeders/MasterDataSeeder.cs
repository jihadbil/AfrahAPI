using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Seeders;

/// <summary>
/// مُنشئ البيانات الأساسية (طرق الدفع، فئات الصالات)
/// </summary>
public class MasterDataSeeder
{
    private readonly ApplicationDbContext _context;

    public MasterDataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// إنشاء البيانات الأساسية
    /// </summary>
    public async Task SeedAsync()
    {
        // 1. طرق الدفع
        await SeedPaymentMethodsAsync();

        // 2. فئات الصالات
        await SeedHallCategoriesAsync();

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// إنشاء طرق الدفع
    /// </summary>
    private async Task SeedPaymentMethodsAsync()
    {
        if (await _context.PaymentMethods.AnyAsync())
            return; // البيانات موجودة بالفعل

        var paymentMethods = new List<PaymentMethod>
        {
            new() { MethodId = Guid.NewGuid(), MethodName = "نقدي" },
            new() { MethodId = Guid.NewGuid(), MethodName = "بطاقة ائتمانية" },
            new() { MethodId = Guid.NewGuid(), MethodName = "تحويل بنكي" },
            new() { MethodId = Guid.NewGuid(), MethodName = "Apple Pay" },
            new() { MethodId = Guid.NewGuid(), MethodName = "STC Pay" },
            new() { MethodId = Guid.NewGuid(), MethodName = "مدى" }
        };

        await _context.PaymentMethods.AddRangeAsync(paymentMethods);
    }

    /// <summary>
    /// إنشاء فئات الصالات
    /// </summary>
    private async Task SeedHallCategoriesAsync()
    {
        if (await _context.HallCategories.AnyAsync())
            return; // البيانات موجودة بالفعل

        var categories = new List<HallCategorie>
        {
            new()
            {
                CategoryID = Guid.NewGuid(),
                CategoryName = "صالات أفراح",
                Description = "صالات مخصصة لحفلات الزفاف والأفراح"
            },
            new()
            {
                CategoryID = Guid.NewGuid(),
                CategoryName = "صالات مؤتمرات",
                Description = "صالات مجهزة للمؤتمرات والفعاليات المهنية"
            },
            new()
            {
                CategoryID = Guid.NewGuid(),
                CategoryName = "صالات اجتماعات",
                Description = "صالات صغيرة للاجتماعات واللقاءات"
            },
            new()
            {
                CategoryID = Guid.NewGuid(),
                CategoryName = "حفلات تخرج",
                Description = "صالات مخصصة لحفلات التخرج والاحتفالات الأكاديمية"
            },
            new()
            {
                CategoryID = Guid.NewGuid(),
                CategoryName = "قاعات مناسبات",
                Description = "قاعات متعددة الأغراض للمناسبات المختلفة"
            },
            new()
            {
                CategoryID = Guid.NewGuid(),
                CategoryName = "صالات VIP",
                Description = "صالات فاخرة للمناسبات الخاصة والحصرية"
            }
        };

        await _context.HallCategories.AddRangeAsync(categories);
    }
}
