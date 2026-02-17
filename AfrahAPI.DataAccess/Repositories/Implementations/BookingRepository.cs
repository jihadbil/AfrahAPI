using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع الحجوزات
/// </summary>
public class BookingRepository : Repository<Booking>, IBookingRepository
{
    /// <summary>
    /// مُنشئ BookingRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId)
    {
        return await _dbSet
            .Include(b => b.Customer)
            .Include(b => b.Hall)
                .ThenInclude(h => h.Owner)
            .Include(b => b.Invoices)
                .ThenInclude(i => i.InvoiceItems)
            .Include(b => b.Invoices)
                .ThenInclude(i => i.Payments)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(Guid customerId)
    {
        return await _dbSet
            .Where(b => b.CustomerId == customerId)
            .Include(b => b.Hall)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetBookingsByHallAsync(Guid hallId)
    {
        return await _dbSet
            .Where(b => b.HallId == hallId)
            .Include(b => b.Customer)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status)
    {
        return await _dbSet
            .Where(b => b.Status == status)
            .Include(b => b.Customer)
            .Include(b => b.Hall)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(b => b.StartDate >= startDate && b.EndDate <= endDate)
            .Include(b => b.Customer)
            .Include(b => b.Hall)
            .OrderBy(b => b.StartDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetPendingBookingsAsync()
    {
        return await GetBookingsByStatusAsync("Pending");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetConfirmedBookingsAsync()
    {
        return await GetBookingsByStatusAsync("Confirmed");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(Guid hallId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(b => b.HallId == hallId && b.StartDate >= today && b.Status == "Confirmed")
            .Include(b => b.Customer)
            .OrderBy(b => b.StartDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetBookingsByDateAsync(Guid hallId, DateTime date)
    {
        return await _dbSet
            .Where(b => b.HallId == hallId && b.StartDate.Date == date.Date)
            .Include(b => b.Customer)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> HasConflictingBookingsAsync(Guid hallId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet.AnyAsync(b => 
            b.HallId == hallId &&
            b.Status != "Cancelled" &&
            b.StartDate < endDate &&
            b.EndDate > startDate);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetBookingsByEventTypeAsync(string eventType)
    {
        return await _dbSet
            .Where(b => b.EventType == eventType)
            .Include(b => b.Customer)
            .Include(b => b.Hall)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalRevenueAsync(Guid? hallId = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.AsQueryable();

        if (hallId.HasValue)
            query = query.Where(b => b.HallId == hallId.Value);

        if (startDate.HasValue)
            query = query.Where(b => b.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(b => b.EndDate <= endDate.Value);

        return await query
            .Where(b => b.Status == "Confirmed" || b.Status == "Completed")
            .SumAsync(b => b.TotalPrice);
    }
}
