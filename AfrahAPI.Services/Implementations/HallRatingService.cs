using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallRating;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة تقييمات الصالات
/// </summary>
public class HallRatingService : BaseService<HallRating, HallRatingCreateDTO, HallRatingReadDTO, HallRatingUpdateDTO>, IHallRatingService
{
    private readonly IHallRatingRepository _hallRatingRepository;
    private readonly IHallRatingSummaryRepository _ratingSummaryRepository;
    private readonly IHallRepository _hallRepository;

    public HallRatingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallRatingRepository hallRatingRepository,
        IHallRatingSummaryRepository ratingSummaryRepository,
        IHallRepository hallRepository)
        : base(unitOfWork, mapper, hallRatingRepository)
    {
        _hallRatingRepository = hallRatingRepository;
        _ratingSummaryRepository = ratingSummaryRepository;
        _hallRepository = hallRepository;
    }

    public async Task<IEnumerable<HallRatingReadDTO>> GetRatingsByHallAsync(Guid hallId)
    {
        var ratings = await _hallRatingRepository.FindAsync(r => r.HallID == hallId);
        return _mapper.Map<IEnumerable<HallRatingReadDTO>>(ratings.OrderByDescending(r => r.CreatedAt));
    }

    public async Task<HallRatingReadDTO> AddRatingAsync(HallRatingCreateDTO createDto)
    {
        var hall = await _hallRepository.GetByIdAsync(createDto.HallID);
        if (hall == null)
            throw new Exception("الصالة غير موجودة");

        // التحقق من أن التقييم بين 1 و 5
        if (createDto.OverallRating < 1 || createDto.OverallRating > 5)
            throw new Exception("التقييم يجب أن يكون بين 1 و 5");

        var rating = _mapper.Map<HallRating>(createDto);
        rating.RatingID = Guid.NewGuid();
        rating.CreatedAt = DateTime.UtcNow;

        await _hallRatingRepository.AddAsync(rating);
        
        // تحديث ملخص التقييمات
        await UpdateRatingSummaryAsync(createDto.HallID);
        
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallRatingReadDTO>(rating);
    }

    public async Task<decimal> CalculateAverageRatingAsync(Guid hallId)
    {
        var ratings = await _hallRatingRepository.FindAsync(r => r.HallID == hallId);
        
        if (!ratings.Any())
            return 0;

        return (decimal)ratings.Average(r => r.OverallRating);
    }

    private async Task UpdateRatingSummaryAsync(Guid hallId)
    {
        var ratings = await _hallRatingRepository.FindAsync(r => r.HallID == hallId);
        var summary = await _ratingSummaryRepository.FirstOrDefaultAsync(s => s.HallID == hallId);

        if (summary == null)
        {
            summary = new HallRatingSummary
            {
                HallRatingSummaryId = Guid.NewGuid(),
                HallID = hallId
            };
            await _ratingSummaryRepository.AddAsync(summary);
        }

        summary.TotalRatingsCount = ratings.Count();
        summary.OverallRatingAverage = ratings.Any() ? (decimal)ratings.Average(r => r.OverallRating) : 0;
        summary.UpdatedAt = DateTime.UtcNow;

        _ratingSummaryRepository.Update(summary);
    }

    public override async Task<HallRatingReadDTO> CreateAsync(HallRatingCreateDTO createDto)
    {
        var rating = _mapper.Map<HallRating>(createDto);
        rating.RatingID = Guid.NewGuid();
        rating.CreatedAt = DateTime.UtcNow;

        await _hallRatingRepository.AddAsync(rating);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallRatingReadDTO>(rating);
    }
}
