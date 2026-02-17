using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع تقييمات الخدمات
/// </summary>
public class ServiceRatingRepository : Repository<ServiceRating>, IServiceRatingRepository
{
    /// <summary>
    /// مُنشئ ServiceRatingRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public ServiceRatingRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServiceRating>> GetRatingsByServiceIdAsync(Guid hallServiceId) =>
        await _dbSet.Where(sr => sr.HallServiceID == hallServiceId).Include(sr => sr.Customer).OrderByDescending(sr => sr.CreatedAt).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<ServiceRating>> GetRatingsByCustomerIdAsync(Guid customerId) =>
        await _dbSet.Where(sr => sr.CustomerID == customerId).Include(sr => sr.HallServices).OrderByDescending(sr => sr.CreatedAt).ToListAsync();

    /// <inheritdoc/>
    public async Task<ServiceRating?> GetRatingWithDetailsAsync(Guid ratingId) =>
        await _dbSet.Include(sr => sr.Customer).Include(sr => sr.HallServices).FirstOrDefaultAsync(sr => sr.ServiceRatingID == ratingId);

    /// <inheritdoc/>
    public async Task<decimal> GetAverageRatingAsync(Guid hallServiceId) =>
        await _dbSet.Where(sr => sr.HallServiceID == hallServiceId).AverageAsync(sr => (decimal)sr.Rating);

    /// <inheritdoc/>
    public async Task<IEnumerable<ServiceRating>> GetLatestRatingsAsync(Guid hallServiceId, int count) =>
        await _dbSet.Where(sr => sr.HallServiceID == hallServiceId).Include(sr => sr.Customer).OrderByDescending(sr => sr.CreatedAt).Take(count).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<ServiceRating>> GetRatingsByScoreAsync(Guid hallServiceId, int rating) =>
        await _dbSet.Where(sr => sr.HallServiceID == hallServiceId && sr.Rating == rating).Include(sr => sr.Customer).ToListAsync();
}
