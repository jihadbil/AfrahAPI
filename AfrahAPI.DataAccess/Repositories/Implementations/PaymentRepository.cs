using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع المدفوعات
/// </summary>
public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    /// <summary>
    /// مُنشئ PaymentRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public PaymentRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetPaymentsByInvoiceIdAsync(Guid invoiceId) =>
        await _dbSet.Where(p => p.InvoiceId == invoiceId).Include(p => p.PaymentMethod).OrderByDescending(p => p.CreatedAt).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(Guid methodId) =>
        await _dbSet.Where(p => p.MethodId == methodId).Include(p => p.Invoice).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status) =>
        await _dbSet.Where(p => p.Status == status).Include(p => p.Invoice).OrderByDescending(p => p.CreatedAt).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate) =>
        await _dbSet.Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate).Include(p => p.Invoice).OrderByDescending(p => p.CreatedAt).ToListAsync();

    /// <inheritdoc/>
    public async Task<Payment?> GetPaymentByReferenceNumberAsync(string referenceNumber) =>
        await _dbSet.Include(p => p.Invoice).FirstOrDefaultAsync(p => p.ReferenceNumber == referenceNumber);

    /// <inheritdoc/>
    public async Task<decimal> GetTotalPaymentsByInvoiceIdAsync(Guid invoiceId) =>
        await _dbSet.Where(p => p.InvoiceId == invoiceId && p.Status == "Completed").SumAsync(p => p.Amount);

    /// <inheritdoc/>
    public async Task<decimal> GetTotalPaymentsInRangeAsync(DateTime startDate, DateTime endDate) =>
        await _dbSet.Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.Status == "Completed").SumAsync(p => p.Amount);

    /// <inheritdoc/>
    public async Task<IEnumerable<Payment>> GetSuccessfulPaymentsAsync() =>
        await GetPaymentsByStatusAsync("Completed");
}
