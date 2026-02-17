using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallUnavailableDate;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة التواريخ غير المتاحة للصالات
/// </summary>
public class HallUnavailableDateService : BaseService<HallUnavailableDate, HallUnavailableDateCreateDTO, HallUnavailableDateReadDTO, HallUnavailableDateUpdateDTO>, IHallUnavailableDateService
{
    private readonly IHallUnavailableDateRepository _unavailableDateRepository;
    private readonly IHallRepository _hallRepository;

    public HallUnavailableDateService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallUnavailableDateRepository unavailableDateRepository,
        IHallRepository hallRepository)
        : base(unitOfWork, mapper, unavailableDateRepository)
    {
        _unavailableDateRepository = unavailableDateRepository;
        _hallRepository = hallRepository;
    }

    public async Task<IEnumerable<HallUnavailableDateReadDTO>> GetUnavailableDatesByHallIdAsync(Guid hallId)
    {
        var dates = await _unavailableDateRepository.FindAsync(d => d.HallID == hallId && d.IsActive);
        return _mapper.Map<IEnumerable<HallUnavailableDateReadDTO>>(dates.OrderBy(d => d.StartDateTime));
    }

    public async Task<HallUnavailableDateReadDTO> AddUnavailableDateAsync(Guid hallId, HallUnavailableDateCreateDTO createDto)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null)
            throw new Exception("الصالة غير موجودة");

        // التحقق من صحة التواريخ
        if (createDto.StartDateTime >= createDto.EndDateTime)
            throw new Exception("تاريخ البدء يجب أن يكون قبل تاريخ الانتهاء");

        var unavailableDate = _mapper.Map<HallUnavailableDate>(createDto);
        unavailableDate.UnavailableID = Guid.NewGuid();
        unavailableDate.HallID = hallId;
        unavailableDate.CreatedAt = DateTime.UtcNow;
        unavailableDate.IsActive = true;

        await _unavailableDateRepository.AddAsync(unavailableDate);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallUnavailableDateReadDTO>(unavailableDate);
    }

    public async Task<bool> RemoveUnavailableDateAsync(Guid unavailableDateId)
    {
        var unavailableDate = await _unavailableDateRepository.GetByIdAsync(unavailableDateId);
        if (unavailableDate == null)
            return false;

        _unavailableDateRepository.Remove(unavailableDate);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public override async Task<HallUnavailableDateReadDTO> CreateAsync(HallUnavailableDateCreateDTO createDto)
    {
        var unavailableDate = _mapper.Map<HallUnavailableDate>(createDto);
        unavailableDate.UnavailableID = Guid.NewGuid();
        unavailableDate.CreatedAt = DateTime.UtcNow;

        await _unavailableDateRepository.AddAsync(unavailableDate);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallUnavailableDateReadDTO>(unavailableDate);
    }
}
