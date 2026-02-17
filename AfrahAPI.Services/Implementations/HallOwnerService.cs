using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallOwner;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة أصحاب الصالات
/// </summary>
public class HallOwnerService : BaseService<HallOwner, HallOwnerCreateDTO, HallOwnerReadDTO, HallOwnerUpdateDTO>, IHallOwnerService
{
    private readonly IHallOwnerRepository _hallOwnerRepository;
    private readonly IHallRepository _hallRepository;

    /// <summary>
    /// المُنشئ
    /// </summary>
    public HallOwnerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallOwnerRepository hallOwnerRepository,
        IHallRepository hallRepository)
        : base(unitOfWork, mapper, hallOwnerRepository)
    {
        _hallOwnerRepository = hallOwnerRepository;
        _hallRepository = hallRepository;
    }

    /// <summary>
    /// الحصول على صاحب صالة بواسطة معرف المستخدم
    /// </summary>
    public async Task<HallOwnerReadDTO?> GetHallOwnerByUserIdAsync(Guid userId)
    {
        var owner = await _hallOwnerRepository.FirstOrDefaultAsync(o => o.UserID == userId);
        return owner != null ? _mapper.Map<HallOwnerReadDTO>(owner) : null;
    }

    /// <summary>
    /// الحصول على جميع الصالات المملوكة لهذا المالك
    /// </summary>
    public async Task<IEnumerable<object>> GetHallOwnerHallsAsync(Guid ownerId)
    {
        var halls = await _hallRepository.FindAsync(h => h.OwnerUserID == ownerId);
        return _mapper.Map<IEnumerable<object>>(halls);
    }

    /// <summary>
    /// تحديث ملف صاحب الصالة الشخصي
    /// </summary>
    public async Task<HallOwnerReadDTO?> UpdateProfileAsync(Guid ownerId, HallOwnerUpdateDTO updateDto)
    {
        var owner = await _hallOwnerRepository.GetByIdAsync(ownerId);
        if (owner == null)
            return null;

        _mapper.Map(updateDto, owner);
        owner.UpdatedAt = DateTime.UtcNow;

        _hallOwnerRepository.Update(owner);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallOwnerReadDTO>(owner);
    }

    /// <summary>
    /// الحصول على إحصائيات صاحب الصالة
    /// </summary>
    public async Task<object> GetOwnerStatisticsAsync(Guid ownerId)
    {
        var owner = await _hallOwnerRepository.GetByIdAsync(ownerId);
        if (owner == null)
            return new { };

        var halls = await _hallRepository.FindAsync(h => h.OwnerUserID == ownerId);
        var hallsList = halls.ToList();

        // حساب الإحصائيات
        var totalHalls = hallsList.Count;
        var verifiedHalls = hallsList.Count(h => h.IsVerified);
        var totalBookings = hallsList.SelectMany(h => h.Bookings).Count();

        return new
        {
            TotalHalls = totalHalls,
            VerifiedHalls = verifiedHalls,
            UnverifiedHalls = totalHalls - verifiedHalls,
            TotalBookings = totalBookings,
            ActiveHalls = hallsList.Count(h => h.IsAvailable)
        };
    }

    /// <summary>
    /// إنشاء صاحب صالة جديد
    /// </summary>
    public override async Task<HallOwnerReadDTO> CreateAsync(HallOwnerCreateDTO createDto)
    {
        var owner = _mapper.Map<HallOwner>(createDto);
        owner.CreatedAt = DateTime.UtcNow;
        owner.OwnerID = Guid.NewGuid();

        await _hallOwnerRepository.AddAsync(owner);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallOwnerReadDTO>(owner);
    }
}
