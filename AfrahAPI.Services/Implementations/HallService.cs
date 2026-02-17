using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Hall;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة الصالات
/// </summary>
public class HallService : BaseService<Hall, HallCreateDTO, HallReadDTO, HallUpdateDTO>, IHallService
{
    private readonly IHallRepository _hallRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IHallUnavailableDateRepository _unavailableDateRepository;

    /// <summary>
    /// المُنشئ
    /// </summary>
    public HallService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallRepository hallRepository,
        IBookingRepository bookingRepository,
        IHallUnavailableDateRepository unavailableDateRepository)
        : base(unitOfWork, mapper, hallRepository)
    {
        _hallRepository = hallRepository;
        _bookingRepository = bookingRepository;
        _unavailableDateRepository = unavailableDateRepository;
    }

    /// <summary>
    /// البحث عن الصالات مع تطبيق الفلاتر
    /// </summary>
    public async Task<IEnumerable<HallReadDTO>> SearchHallsAsync(HallSearchDTO searchDto)
    {
        var halls = await _hallRepository.GetAllAsync();
        var query = halls.AsQueryable();

        // تطبيق الفلاتر
        if (!string.IsNullOrWhiteSpace(searchDto.Keyword))
        {
            query = query.Where(h =>
                h.HallName.Contains(searchDto.Keyword, StringComparison.OrdinalIgnoreCase) ||
                (h.Description != null && h.Description.Contains(searchDto.Keyword, StringComparison.OrdinalIgnoreCase)));
        }

        if (searchDto.CategoryId.HasValue)
        {
            query = query.Where(h => h.CategoryID == searchDto.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchDto.City))
        {
            query = query.Where(h => h.Address != null && h.Address.Contains(searchDto.City, StringComparison.OrdinalIgnoreCase));
        }

        if (searchDto.MinCapacity.HasValue)
        {
            query = query.Where(h => h.Capacity >= searchDto.MinCapacity.Value);
        }

        if (searchDto.MaxCapacity.HasValue)
        {
            query = query.Where(h => h.Capacity <= searchDto.MaxCapacity.Value);
        }

        if (searchDto.MinPrice.HasValue)
        {
            query = query.Where(h => h.PricePerDay >= searchDto.MinPrice.Value || h.PricePerHour >= searchDto.MinPrice.Value);
        }

        if (searchDto.MaxPrice.HasValue)
        {
            query = query.Where(h => h.PricePerDay <= searchDto.MaxPrice.Value || h.PricePerHour <= searchDto.MaxPrice.Value);
        }

        if (searchDto.OnlyAvailable == true)
        {
            query = query.Where(h => h.IsAvailable);
        }

        if (searchDto.OnlyVerified == true)
        {
            query = query.Where(h => h.IsVerified);
        }

        // فلترة حسب التوفر في التواريخ المحددة
        if (searchDto.StartDate.HasValue && searchDto.EndDate.HasValue)
        {
            var availableHalls = new List<Hall>();
            foreach (var hall in query)
            {
                if (await CheckHallAvailabilityAsync(hall.HallID, searchDto.StartDate.Value, searchDto.EndDate.Value))
                {
                    availableHalls.Add(hall);
                }
            }
            return _mapper.Map<IEnumerable<HallReadDTO>>(availableHalls);
        }

        return _mapper.Map<IEnumerable<HallReadDTO>>(query.ToList());
    }

    /// <summary>
    /// الحصول على الصالات المتاحة لنطاق تواريخ معين
    /// </summary>
    public async Task<IEnumerable<HallReadDTO>> GetAvailableHallsAsync(DateTime startDate, DateTime endDate)
    {
        var allHalls = await _hallRepository.FindAsync(h => h.IsAvailable);
        var availableHalls = new List<Hall>();

        foreach (var hall in allHalls)
        {
            if (await CheckHallAvailabilityAsync(hall.HallID, startDate, endDate))
            {
                availableHalls.Add(hall);
            }
        }

        return _mapper.Map<IEnumerable<HallReadDTO>>(availableHalls);
    }

    /// <summary>
    /// فحص توفر صالة معينة في نطاق تواريخ محدد
    /// </summary>
    public async Task<bool> CheckHallAvailabilityAsync(Guid hallId, DateTime startDate, DateTime endDate)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null || !hall.IsAvailable)
            return false;

        // فحص التواريخ المحجوبة
        var unavailableDates = await _unavailableDateRepository.FindAsync(
            ud => ud.HallID == hallId &&
                  ((ud.StartDateTime <= endDate && ud.EndDateTime >= startDate)));

        if (unavailableDates.Any())
            return false;

        // فحص الحجوزات الموجودة
        var existingBookings = await _bookingRepository.FindAsync(
            b => b.HallId == hallId &&
                 b.Status != "Cancelled" &&
                 ((b.StartDate <= endDate && b.EndDate >= startDate)));

        if (existingBookings.Any())
            return false;

        return true;
    }

    /// <summary>
    /// الحصول على جميع صالات مالك معين
    /// </summary>
    public async Task<IEnumerable<HallReadDTO>> GetHallsByOwnerAsync(Guid ownerId)
    {
        var halls = await _hallRepository.FindAsync(h => h.OwnerUserID == ownerId);
        return _mapper.Map<IEnumerable<HallReadDTO>>(halls);
    }

    /// <summary>
    /// الحصول على الصالات حسب الفئة
    /// </summary>
    public async Task<IEnumerable<HallReadDTO>> GetHallsByCategoryAsync(Guid categoryId)
    {
        var halls = await _hallRepository.FindAsync(h => h.CategoryID == categoryId);
        return _mapper.Map<IEnumerable<HallReadDTO>>(halls);
    }

    /// <summary>
    /// التحقق من الصالة (للمسؤول فقط)
    /// </summary>
    public async Task<HallReadDTO?> VerifyHallAsync(Guid hallId)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null)
            return null;

        hall.IsVerified = true;
        hall.UpdatedAt = DateTime.UtcNow;

        _hallRepository.Update(hall);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallReadDTO>(hall);
    }

    /// <summary>
    /// تبديل حالة توفر الصالة
    /// </summary>
    public async Task<HallReadDTO?> UpdateHallAvailabilityAsync(Guid hallId, bool isAvailable)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null)
            return null;

        hall.IsAvailable = isAvailable;
        hall.UpdatedAt = DateTime.UtcNow;

        _hallRepository.Update(hall);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallReadDTO>(hall);
    }

    /// <summary>
    /// إنشاء صالة جديدة
    /// </summary>
    public override async Task<HallReadDTO> CreateAsync(HallCreateDTO createDto)
    {
        var hall = _mapper.Map<Hall>(createDto);
        hall.HallID = Guid.NewGuid();
        hall.CreatedAt = DateTime.UtcNow;
        hall.IsVerified = false; // الصالة تحتاج للتحقق من المسؤول

        await _hallRepository.AddAsync(hall);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallReadDTO>(hall);
    }
}
