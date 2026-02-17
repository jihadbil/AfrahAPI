using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع التواريخ غير المتاحة للصالات
/// </summary>
public class HallUnavailableDateRepository : Repository<HallUnavailableDate>, IHallUnavailableDateRepository
{
    /// <summary>
    /// مُنشئ HallUnavailableDateRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallUnavailableDateRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<HallUnavailableDate>> GetUnavailableDatesByHallIdAsync(Guid hallId) =>
        await _dbSet.Where(hud => hud.HallID == hallId).OrderBy(hud => hud.StartDateTime).ToListAsync();

    /// <inheritdoc/>
    public async Task<bool> IsHallAvailableAsync(Guid hallId, DateTime date) =>
        !await _dbSet.AnyAsync(hud => hud.HallID == hallId && hud.StartDateTime.Date <= date.Date && hud.EndDateTime.Date >= date.Date);

    /// <inheritdoc/>
    public async Task<bool> IsHallAvailableInRangeAsync(Guid hallId, DateTime startDate, DateTime endDate) =>
        !await _dbSet.AnyAsync(hud => hud.HallID == hallId && hud.StartDateTime <= endDate && hud.EndDateTime >= startDate);

    /// <inheritdoc/>
    public async Task<IEnumerable<HallUnavailableDate>> GetUnavailableDatesInRangeAsync(Guid hallId, DateTime startDate, DateTime endDate) =>
        await _dbSet.Where(hud => hud.HallID == hallId && hud.StartDateTime <= endDate && hud.EndDateTime >= startDate).ToListAsync();

    /// <inheritdoc/>
    public async Task RemoveExpiredDatesAsync(Guid hallId)
    {
        var today = DateTime.UtcNow.Date;
        var expired = await _dbSet.Where(hud => hud.HallID == hallId && hud.EndDateTime < today).ToListAsync();
        _dbSet.RemoveRange(expired);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HallUnavailableDate>> GetFutureUnavailableDatesAsync(Guid hallId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet.Where(hud => hud.HallID == hallId && hud.StartDateTime >= today).OrderBy(hud => hud.StartDateTime).ToListAsync();
    }
}
