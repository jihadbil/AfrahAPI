using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع أصحاب الصالات
/// </summary>
public class HallOwnerRepository : Repository<HallOwner>, IHallOwnerRepository
{
    /// <summary>
    /// مُنشئ HallOwnerRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallOwnerRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// جلب مالك صالة مع جميع صالاته وتفاصيلها
    /// </summary>
    /// <param name="ownerId">معرف المالك</param>
    /// <returns>المالك مع صالاته أو null</returns>
    public async Task<HallOwner?> GetOwnerWithHallsAsync(Guid ownerId)
    {
        return await _dbSet
            .Include(ho => ho.Halls)
                .ThenInclude(h => h.Category)
            .Include(ho => ho.Halls)
                .ThenInclude(h => h.HallRatingSummary)
            .FirstOrDefaultAsync(ho => ho.OwnerID == ownerId);
    }

    /// <summary>
    /// جلب جميع ملاك الصالات في دولة معينة
    /// </summary>
    /// <param name="country">اسم الدولة</param>
    /// <returns>قائمة بملاك الصالات</returns>
    public async Task<IEnumerable<HallOwner>> GetOwnersByCountryAsync(string country)
    {
        return await _dbSet
            .Where(ho => ho.Country == country)
            .ToListAsync();
    }

    /// <summary>
    /// جلب جميع ملاك الصالات في مدينة معينة
    /// </summary>
    /// <param name="city">اسم المدينة</param>
    /// <returns>قائمة بملاك الصالات</returns>
    public async Task<IEnumerable<HallOwner>> GetOwnersByCityAsync(string city)
    {
        return await _dbSet
            .Where(ho => ho.City == city)
            .ToListAsync();
    }

    /// <summary>
    /// البحث عن ملاك الصالات بالاسم
    /// </summary>
    /// <param name="name">النص المراد البحث عنه</param>
    /// <returns>قائمة بالملاك المطابقين</returns>
    public async Task<IEnumerable<HallOwner>> SearchOwnersByNameAsync(string name)
    {
        return await _dbSet
            .Where(ho => ho.FirstName.Contains(name) || ho.LastName.Contains(name))
            .ToListAsync();
    }

    /// <summary>
    /// جلب مالك صالة بواسطة معرف المستخدم
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>المالك أو null</returns>
    public async Task<HallOwner?> GetOwnerByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ho => ho.UserID == userId);
    }

    /// <summary>
    /// جلب مالك صالة بواسطة البريد الإلكتروني
    /// </summary>
    /// <param name="email">البريد الإلكتروني</param>
    /// <returns>المالك أو null</returns>
    public async Task<HallOwner?> GetOwnerByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ho => ho.Email == email);
    }

    /// <summary>
    /// جلب جميع ملاك الصالات النشطين
    /// </summary>
    /// <returns>قائمة بالملاك النشطين</returns>
    public async Task<IEnumerable<HallOwner>> GetActiveOwnersAsync()
    {
        return await _dbSet
            .ToListAsync();
    }
}
