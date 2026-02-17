using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع بنود الفاتورة
/// </summary>
public class InvoiceItemsRepository : Repository<InvoiceItems>, IInvoiceItemsRepository
{
    /// <summary>
    /// مُنشئ InvoiceItemsRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public InvoiceItemsRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<InvoiceItems>> GetItemsByInvoiceIdAsync(Guid invoiceId) =>
        await _dbSet.Where(ii => ii.InvoiceId == invoiceId).Include(ii => ii.HallService).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<InvoiceItems>> GetItemsByServiceIdAsync(Guid hallServiceId) =>
        await _dbSet.Where(ii => ii.HallServiceId == hallServiceId).Include(ii => ii.Invoice).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<InvoiceItems>> GetItemsByTypeAsync(Guid invoiceId, string itemType) =>
        await _dbSet.Where(ii => ii.InvoiceId == invoiceId && ii.ItemType == itemType).ToListAsync();

    /// <inheritdoc/>
    public async Task<decimal> GetTotalAmountByInvoiceIdAsync(Guid invoiceId) =>
        await _dbSet.Where(ii => ii.InvoiceId == invoiceId).SumAsync(ii => ii.Total);

    /// <inheritdoc/>
    public async Task RemoveItemsByInvoiceIdAsync(Guid invoiceId)
    {
        var items = await _dbSet.Where(ii => ii.InvoiceId == invoiceId).ToListAsync();
        _dbSet.RemoveRange(items);
    }
}
