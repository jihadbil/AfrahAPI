using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.ServiceRating;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة تقييمات الخدمات
/// </summary>
public class ServiceRatingService : BaseService<ServiceRating, ServiceRatingCreateDTO, ServiceRatingReadDTO, ServiceRatingUpdateDTO>, IServiceRatingService
{
    private readonly IServiceRatingRepository _serviceRatingRepository;
    private readonly IServiceRatingSummaryRepository _ratingSummaryRepository;
    private readonly IHallServicesRepository _hallServicesRepository;

    public ServiceRatingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceRatingRepository serviceRatingRepository,
        IServiceRatingSummaryRepository ratingSummaryRepository,
        IHallServicesRepository hallServicesRepository)
        : base(unitOfWork, mapper, serviceRatingRepository)
    {
        _serviceRatingRepository = serviceRatingRepository;
        _ratingSummaryRepository = ratingSummaryRepository;
        _hallServicesRepository = hallServicesRepository;
    }

    public async Task<IEnumerable<ServiceRatingReadDTO>> GetRatingsByServiceAsync(Guid serviceId)
    {
        var ratings = await _serviceRatingRepository.FindAsync(r => r.HallServiceID == serviceId);
        return _mapper.Map<IEnumerable<ServiceRatingReadDTO>>(ratings.OrderByDescending(r => r.CreatedAt));
    }

    public async Task<ServiceRatingReadDTO> AddRatingAsync(ServiceRatingCreateDTO createDto)
    {
        var service = await _hallServicesRepository.GetByIdAsync(createDto.HallServiceID);
        if (service == null)
            throw new Exception("الخدمة غير موجودة");

        // التحقق من أن التقييم بين 1 و 5
        if (createDto.Rating < 1 || createDto.Rating > 5)
            throw new Exception("التقييم يجب أن يكون بين 1 و 5");

        var rating = _mapper.Map<ServiceRating>(createDto);
        rating.ServiceRatingID = Guid.NewGuid();
        rating.CreatedAt = DateTime.UtcNow;

        await _serviceRatingRepository.AddAsync(rating);
        
        // تحديث ملخص التقييمات
        await UpdateRatingSummaryAsync(createDto.HallServiceID);
        
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<ServiceRatingReadDTO>(rating);
    }

    public async Task<decimal> CalculateAverageRatingAsync(Guid serviceId)
    {
        var ratings = await _serviceRatingRepository.FindAsync(r => r.HallServiceID == serviceId);
        
        if (!ratings.Any())
            return 0;

        return (decimal)ratings.Average(r => r.Rating);
    }

    private async Task UpdateRatingSummaryAsync(Guid serviceId)
    {
        var ratings = await _serviceRatingRepository.FindAsync(r => r.HallServiceID == serviceId);
        var summary = await _ratingSummaryRepository.FirstOrDefaultAsync(s => s.HallServiceID == serviceId);

        if (summary == null)
        {
            summary = new ServiceRatingSummary
            {
                ServiceRatingSummaryId = Guid.NewGuid(),
                HallServiceID = serviceId
            };
            await _ratingSummaryRepository.AddAsync(summary);
        }

        summary.TotalRatingsCount = ratings.Count();
        summary.RatingAverage = ratings.Any() ? (decimal)ratings.Average(r => r.Rating) : 0;
        summary.UpdatedAt = DateTime.UtcNow;

        _ratingSummaryRepository.Update(summary);
    }

    public override async Task<ServiceRatingReadDTO> CreateAsync(ServiceRatingCreateDTO createDto)
    {
        var rating = _mapper.Map<ServiceRating>(createDto);
        rating.ServiceRatingID = Guid.NewGuid();
        rating.CreatedAt = DateTime.UtcNow;

        await _serviceRatingRepository.AddAsync(rating);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<ServiceRatingReadDTO>(rating);
    }
}
