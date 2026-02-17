using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Seeders;

/// <summary>
/// مُنشئ بيانات الصالات والخدمات (نسخة مبسطة)
/// </summary>
public class HallSeeder
{
    private readonly ApplicationDbContext _context;
    private List<Guid> _categoryIds = new();
    private List<Guid> _ownerIds = new();
    private List<Guid> _paymentMethodIds = new();

    public HallSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// إنشاء بيانات الصالات
    /// </summary>
    public async Task SeedAsync()
    {
        // جلب البيانات الأساسية
        _categoryIds = await _context.HallCategories.Select(c => c.CategoryID).ToListAsync();
        _ownerIds = await _context.HallOwners.Select(o => o.OwnerID).ToListAsync();
        _paymentMethodIds = await _context.PaymentMethods.Select(p => p.MethodId).ToListAsync();

        if (!_categoryIds.Any() || !_ownerIds.Any())
            return; // لا توجد بيانات أساسية

        // 1. إنشاء الصالات
        var hallIds = await SeedHallsAsync();

        // 2. إنشاء خدمات الصالات
        await SeedHallServicesAsync(hallIds);

        // 3. ربط طرق الدفع بالصالات
        await SeedHallPaymentMethodsAsync(hallIds);

        await _context.SaveChangesAsync();
    }

    private async Task<List<Guid>> SeedHallsAsync()
    {
        if (await _context.Halls.AnyAsync())
            return await _context.Halls.Select(h => h.HallID).ToListAsync();

        var halls = new List<Hall>();

        halls.Add(new Hall
        {
            HallID = Guid.NewGuid(),
            HallName = "قصر الأفراح الملكي",
            Description = "صالة فاخرة للأفراح والمناسبات الكبرى مع ديكورات ملكية",
            Address = "طريق الملك فهد، الرياض",
            Capacity = 500,
            PricePerDay = 25000m,
            CategoryID = _categoryIds[0],
            OwnerUserID = _ownerIds[0],
            CreatedAt = DateTime.UtcNow
        });

        halls.Add(new Hall
        {
            HallID = Guid.NewGuid(),
            HallName = "قاعة النخبة للمؤتمرات",
            Description = "قاعة مجهزة بأحدث التقنيات للمؤتمرات والفعاليات",
            Address = "شارع التحلية، جدة",
            Capacity = 300,
            PricePerDay = 15000m,
            CategoryID = _categoryIds.Count > 1 ? _categoryIds[1] : _categoryIds[0],
            OwnerUserID = _ownerIds.Count > 1 ? _ownerIds[1] : _ownerIds[0],
            CreatedAt = DateTime.UtcNow
        });

        halls.Add(new Hall
        {
            HallID = Guid.NewGuid(),
            HallName = "صالة الياسمين",
            Description = "صالة أنيقة ومريحة للمناسبات الخاصة",
            Address = "حي الملقا، الرياض",
            Capacity = 200,
            PricePerDay = 10000m,
            CategoryID = _categoryIds[0],
            OwnerUserID = _ownerIds[0],
            CreatedAt = DateTime.UtcNow
        });

        await _context.Halls.AddRangeAsync(halls);
        await _context.SaveChangesAsync();

        // إنشاء ملخصات التقييمات للصالات
        var ratingSummaries = halls.Select(h => new HallRatingSummary
        {
            HallRatingSummaryId = Guid.NewGuid(),
            HallID = h.HallID,
            OverallRatingAverage = 0m
        }).ToList();

        await _context.HallRatingSummaries.AddRangeAsync(ratingSummaries);

        return halls.Select(h => h.HallID).ToList();
    }

    private async Task SeedHallServicesAsync(List<Guid> hallIds)
    {
        if (await _context.HallServices.AnyAsync())
            return;

        var services = new List<HallServices>();
        string[] serviceNames = { "ديكور", "تصوير فوتوغرافي", "تصوير فيديو", "طعام", "حلويات", "موسيقى" };
        decimal[] prices = { 5000m, 3000m, 4000m, 8000m, 2000m, 6000m };

        foreach (var hallId in hallIds)
        {
            for (int i = 0; i < serviceNames.Length; i++)
            {
                services.Add(new HallServices
                {
                    ServiceId = Guid.NewGuid(),
                    HallID = hallId,
                    ServiceName = serviceNames[i],
                    Description = $"خدمة {serviceNames[i]} احترافية",
                    Price = prices[i],
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _context.HallServices.AddRangeAsync(services);
        await _context.SaveChangesAsync();

        // إنشاء ملخصات التقييمات للخدمات  
        var serviceRatingSummaries = services.Select(s => new ServiceRatingSummary
        {
            ServiceRatingSummaryId = Guid.NewGuid(),
            HallServiceID = s.ServiceId,
            RatingAverage = 0m
        }).ToList();

        await _context.ServiceRatingSummaries.AddRangeAsync(serviceRatingSummaries);
    }

    private async Task SeedHallPaymentMethodsAsync(List<Guid> hallIds)
    {
        if (await _context.HallPaymentMethods.AnyAsync())
            return;

        var hallPaymentMethods = new List<HallPaymentMethod>();

        foreach (var hallId in hallIds)
        {
            foreach (var paymentMethodId in _paymentMethodIds)
            {
                hallPaymentMethods.Add(new HallPaymentMethod
                {
                    HallID = hallId,
                    PaymentMethodID = paymentMethodId,
                    IsActive = true
                });
            }
        }

        await _context.HallPaymentMethods.AddRangeAsync(hallPaymentMethods);
    }
}
