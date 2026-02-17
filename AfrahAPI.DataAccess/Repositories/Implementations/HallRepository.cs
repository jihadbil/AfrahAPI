using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع الصالات
/// </summary>
public class HallRepository : Repository<Hall>, IHallRepository
{
    /// <summary>
    /// مُنشئ HallRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Hall?> GetHallWithDetailsAsync(Guid hallId)
    {
        return await _dbSet
            .Include(h => h.Owner)
            .Include(h => h.Category)
            .Include(h => h.HallMedia)
            .Include(h => h.HallServices)
            .Include(h => h.HallRatings)
                .ThenInclude(hr => hr.Customer)
            .Include(h => h.HallRatingSummary)
            .Include(h => h.HallPaymentMethods)
                .ThenInclude(hpm => hpm.PaymentMethod)
            .FirstOrDefaultAsync(h => h.HallID == hallId);
    }

    /// <inheritdoc/>
    public async Task<Hall?> GetHallWithMediaAsync(Guid hallId)
    {
        return await _dbSet
            .Include(h => h.HallMedia)
            .FirstOrDefaultAsync(h => h.HallID == hallId);
    }

    /// <inheritdoc/>
    public async Task<Hall?> GetHallWithServicesAsync(Guid hallId)
    {
        return await _dbSet
            .Include(h => h.HallServices)
                .ThenInclude(hs => hs.ServiceRatingSummary)
            .FirstOrDefaultAsync(h => h.HallID == hallId);
    }

    /// <inheritdoc/>
    public async Task<Hall?> GetHallWithRatingsAsync(Guid hallId)
    {
        return await _dbSet
            .Include(h => h.HallRatings)
                .ThenInclude(hr => hr.Customer)
            .Include(h => h.HallRatingSummary)
            .FirstOrDefaultAsync(h => h.HallID == hallId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> GetHallsByCategoryAsync(Guid categoryId)
    {
        return await _dbSet
            .Where(h => h.CategoryID == categoryId)
            .Include(h => h.HallRatingSummary)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> GetHallsByOwnerAsync(Guid ownerId)
    {
        return await _dbSet
            .Where(h => h.OwnerUserID == ownerId)
            .Include(h => h.Category)
            .Include(h => h.HallRatingSummary)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> GetAvailableHallsAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(h => !h.HallUnavailableDates.Any(ud => 
                (ud.StartDateTime <= endDate && ud.EndDateTime >= startDate)))
            .Include(h => h.HallRatingSummary)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> SearchHallsAsync(string searchTerm)
    {
        return await _dbSet
            .Where(h => h.HallName.Contains(searchTerm) || 
                        h.Description.Contains(searchTerm) ||
                        h.Address.Contains(searchTerm))
            .Include(h => h.HallRatingSummary)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> GetActiveHallsAsync()
    {
        return await _dbSet
            .Include(h => h.Category)
            .Include(h => h.HallRatingSummary)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> GetHallsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _dbSet
            .Where(h => (h.PricePerDay >= minPrice && h.PricePerDay <= maxPrice) ||
                        (h.PricePerHour >= minPrice && h.PricePerHour <= maxPrice))
            .Include(h => h.HallRatingSummary)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> GetHallsByCapacityAsync(int minCapacity)
    {
        return await _dbSet
            .Where(h => h.Capacity >= minCapacity)
            .Include(h => h.HallRatingSummary)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hall>> GetTopRatedHallsAsync(int topCount)
    {
        return await _dbSet
            .Include(h => h.HallRatingSummary)
            .Where(h => h.HallRatingSummary != null)
            .OrderByDescending(h => h.HallRatingSummary!.OverallRatingAverage)
            .Take(topCount)
            .ToListAsync();
    }
}
