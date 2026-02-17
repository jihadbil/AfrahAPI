using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع العملاء
/// </summary>
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    /// <summary>
    /// مُنشئ CustomerRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// جلب عميل مع جميع حجوزاته والصالات المرتبطة
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>العميل مع حجوزاته أو null</returns>
    public async Task<Customer?> GetCustomerWithBookingsAsync(Guid customerId)
    {
        return await _dbSet
            .Include(c => c.Bookings)
                .ThenInclude(b => b.Hall)
            .FirstOrDefaultAsync(c => c.CustomerID == customerId);
    }

    /// <summary>
    /// جلب عميل مع جميع تقييماته للصالات والخدمات
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>العميل مع تقييماته أو null</returns>
    public async Task<Customer?> GetCustomerWithRatingsAsync(Guid customerId)
    {
        return await _dbSet
            .Include(c => c.HallRatings)
                .ThenInclude(hr => hr.Hall)
            .Include(c => c.ServiceRatings)
                .ThenInclude(sr => sr.HallServices)
            .FirstOrDefaultAsync(c => c.CustomerID == customerId);
    }

    /// <summary>
    /// جلب جميع العملاء في دولة معينة
    /// </summary>
    /// <param name="country">اسم الدولة</param>
    /// <returns>قائمة بالعملاء في الدولة</returns>
    public async Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country)
    {
        return await _dbSet
            .Where(c => c.Country == country)
            .ToListAsync();
    }

    /// <summary>
    /// جلب جميع العملاء في مدينة معينة
    /// </summary>
    /// <param name="city">اسم المدينة</param>
    /// <returns>قائمة بالعملاء في المدينة</returns>
    public async Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city)
    {
        return await _dbSet
            .Where(c => c.City == city)
            .ToListAsync();
    }

    /// <summary>
    /// البحث عن العملاء بالاسم الأول أو الأخير
    /// </summary>
    /// <param name="name">النص المراد البحث عنه</param>
    /// <returns>قائمة بالعملاء المطابقين</returns>
    public async Task<IEnumerable<Customer>> SearchCustomersByNameAsync(string name)
    {
        return await _dbSet
            .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
            .ToListAsync();
    }

    /// <summary>
    /// جلب عميل بواسطة معرف المستخدم
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>العميل أو null إذا لم يوجد</returns>
    public async Task<Customer?> GetCustomerByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.UserID == userId);
    }

    /// <summary>
    /// جلب جميع العملاء حسب الجنس
    /// </summary>
    /// <param name="gender">الجنس (ذكر/أنثى)</param>
    /// <returns>قائمة بالعملاء</returns>
    public async Task<IEnumerable<Customer>> GetCustomersByGenderAsync(string gender)
    {
        return await _dbSet
            .Where(c => c.Gender == gender)
            .ToListAsync();
    }
}
