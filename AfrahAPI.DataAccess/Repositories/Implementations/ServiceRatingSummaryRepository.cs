using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع ملخصات تقييمات الخدمات
/// </summary>
public class ServiceRatingSummaryRepository : Repository<ServiceRatingSummary>, IServiceRatingSummaryRepository
{
    /// <summary>
    /// مُنشئ ServiceRatingSummaryRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public ServiceRatingSummaryRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<ServiceRatingSummary?> GetSummaryByServiceIdAsync(Guid hallServiceId) =>
        await _dbSet.Include(srs => srs.HallServices).FirstOrDefaultAsync(srs => srs.HallServices != null && srs.HallServices.ServiceId == hallServiceId);

    /// <inheritdoc/>
    public async Task UpdateSummaryAsync(Guid hallServiceId)
    {
        var ratings = await _context.ServiceRatings.Where(sr => sr.HallServiceID == hallServiceId).ToListAsync();
        if (!ratings.Any()) return;

        var summary = await GetSummaryByServiceIdAsync(hallServiceId);
        if (summary == null) return;

        summary.RatingAverage = (decimal)ratings.Average(r => r.Rating);
        _dbSet.Update(summary);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServiceRatingSummary>> GetTopRatedSummariesAsync(int topCount) =>
        await _dbSet.OrderByDescending(srs => srs.RatingAverage).Take(topCount).ToListAsync();
}
