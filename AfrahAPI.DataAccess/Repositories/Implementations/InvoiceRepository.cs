using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع الفواتير
/// </summary>
public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    /// <summary>
    /// مُنشئ InvoiceRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public InvoiceRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Invoice?> GetInvoiceWithDetailsAsync(Guid invoiceId)
    {
        return await _dbSet
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
                .ThenInclude(b => b.Hall)
            .Include(i => i.InvoiceItems)
                . ThenInclude(ii => ii.HallService)
            .Include(i => i.Payments)
                .ThenInclude(p => p.PaymentMethod)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
    }

    /// <inheritdoc/>
    public async Task<Invoice?> GetInvoiceByNumberAsync(string invoiceNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetInvoicesByBookingAsync(Guid bookingId)
    {
        return await _dbSet
            .Where(i => i.BookingId == bookingId)
            .Include(i => i.InvoiceItems)
            .Include(i => i.Payments)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetInvoicesByStatusAsync(string status)
    {
        return await _dbSet
            .Where(i => i.Status == status)
            .Include(i => i.Booking)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetUnpaidInvoicesAsync()
    {
        return await GetInvoicesByStatusAsync("Unpaid");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetPartiallyPaidInvoicesAsync()
    {
        return await GetInvoicesByStatusAsync("PartiallyPaid");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(i => i.DueDate < today && i.Status != "Paid")
            .Include(i => i.Booking)
            .OrderBy(i => i.DueDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate)
            .Include(i => i.Booking)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalOutstandingAmountAsync()
    {
        return await _dbSet
            .Where(i => i.Status != "Paid")
            .SumAsync(i => i.BalanceDue);
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalCommissionAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(i => i.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(i => i.CreatedAt <= endDate.Value);

        return await query.SumAsync(i => i.CommissionAmount);
    }
}
