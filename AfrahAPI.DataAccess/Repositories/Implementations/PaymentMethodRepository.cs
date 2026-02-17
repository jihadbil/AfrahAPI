using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع وسائل الدفع
/// </summary>
public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
{
    /// <summary>
    /// مُنشئ PaymentMethodRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public PaymentMethodRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<PaymentMethod?> GetPaymentMethodByNameAsync(string methodName) =>
        await _dbSet.FirstOrDefaultAsync(pm => pm.MethodName == methodName);

    /// <inheritdoc/>
    public async Task<IEnumerable<PaymentMethod>> GetActivePaymentMethodsAsync() =>
        await _dbSet.ToListAsync();

    /// <inheritdoc/>
    public async Task<PaymentMethod?> GetPaymentMethodWithHallsAsync(Guid methodId) =>
        await _dbSet.Include(pm => pm.HallPaymentMethods).ThenInclude(hpm => hpm.Hall).FirstOrDefaultAsync(pm => pm.MethodId == methodId);
}
