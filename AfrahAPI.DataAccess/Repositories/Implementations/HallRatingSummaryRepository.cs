using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع ملخصات تقييمات الصالات
/// </summary>
public class HallRatingSummaryRepository : Repository<HallRatingSummary>, IHallRatingSummaryRepository
{
    /// <summary>
    /// مُنشئ HallRatingSummaryRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallRatingSummaryRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<HallRatingSummary?> GetSummaryByHallIdAsync(Guid hallId) =>
        await _dbSet.Include(hrs => hrs.Hall).FirstOrDefaultAsync(hrs => hrs.Hall != null && hrs.Hall.HallID == hallId);

    /// <inheritdoc/>
    public async Task UpdateSummaryAsync(Guid hallId)
    {
        var ratings = await _context.HallRatings.Where(hr => hr.HallID == hallId).ToListAsync();
        if (!ratings.Any()) return;

        var summary = await GetSummaryByHallIdAsync(hallId);
        if (summary == null) return;

        summary.OverallRatingAverage = (decimal)ratings.Average(r => r.OverallRating);
        _dbSet.Update(summary);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HallRatingSummary>> GetTopRatedSummariesAsync(int topCount) =>
        await _dbSet.OrderByDescending(hrs => hrs.OverallRatingAverage).Take(topCount).ToListAsync();
}
