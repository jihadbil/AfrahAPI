using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع طرق الدفع المتاحة للصالات
/// </summary>
public class HallPaymentMethodRepository : Repository<HallPaymentMethod>, IHallPaymentMethodRepository
{
    /// <summary>
    /// مُنشئ HallPaymentMethodRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallPaymentMethodRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<HallPaymentMethod>> GetPaymentMethodsByHallIdAsync(Guid hallId) =>
        await _dbSet.Where(hpm => hpm.HallID == hallId).Include(hpm => hpm.PaymentMethod).ToListAsync();

    /// <inheritdoc/>
    public async Task<bool> IsPaymentMethodAvailableAsync(Guid hallId, Guid paymentMethodId) =>
        await _dbSet.AnyAsync(hpm => hpm.HallID == hallId && hpm.PaymentMethodID == paymentMethodId && hpm.IsActive);

    /// <inheritdoc/>
    public async Task AddPaymentMethodToHallAsync(Guid hallId, Guid paymentMethodId)
    {
        var exists = await _dbSet.AnyAsync(hpm => hpm.HallID == hallId && hpm.PaymentMethodID == paymentMethodId);
        if (!exists)
            await _dbSet.AddAsync(new HallPaymentMethod { HallID = hallId, PaymentMethodID = paymentMethodId, IsActive = true });
    }

    /// <inheritdoc/>
    public async Task RemovePaymentMethodFromHallAsync(Guid hallId, Guid paymentMethodId)
    {
        var hpm = await _dbSet.FirstOrDefaultAsync(h => h.HallID == hallId && h.PaymentMethodID == paymentMethodId);
        if (hpm != null) _dbSet.Remove(hpm);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HallPaymentMethod>> GetHallsByPaymentMethodAsync(Guid paymentMethodId) =>
        await _dbSet.Where(hpm => hpm.PaymentMethodID == paymentMethodId).Include(hpm => hpm.Hall).ToListAsync();
}
