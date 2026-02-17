using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallServices;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة خدمات الصالات
/// </summary>
public class HallServicesService : BaseService<HallServices, HallServicesCreateDTO, HallServicesReadDTO, HallServicesUpdateDTO>, IHallServicesService
{
    private readonly IHallServicesRepository _hallServicesRepository;
    private readonly IHallRepository _hallRepository;

    public HallServicesService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallServicesRepository hallServicesRepository,
        IHallRepository hallRepository)
        : base(unitOfWork, mapper, hallServicesRepository)
    {
        _hallServicesRepository = hallServicesRepository;
        _hallRepository = hallRepository;
    }

    public async Task<IEnumerable<HallServicesReadDTO>> GetServicesByHallIdAsync(Guid hallId)
    {
        var services = await _hallServicesRepository.FindAsync(s => s.HallID == hallId);
        return _mapper.Map<IEnumerable<HallServicesReadDTO>>(services);
    }

    public async Task<HallServicesReadDTO> AddServiceToHallAsync(Guid hallId, HallServicesCreateDTO createDto)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null)
            throw new Exception("الصالة غير موجودة");

        var service = _mapper.Map<HallServices>(createDto);
        service.ServiceId = Guid.NewGuid();
        service.HallID = hallId;
        service.CreatedAt = DateTime.UtcNow;

        await _hallServicesRepository.AddAsync(service);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallServicesReadDTO>(service);
    }

    public override async Task<HallServicesReadDTO> CreateAsync(HallServicesCreateDTO createDto)
    {
        var service = _mapper.Map<HallServices>(createDto);
        service.ServiceId = Guid.NewGuid();
        service.CreatedAt = DateTime.UtcNow;

        await _hallServicesRepository.AddAsync(service);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallServicesReadDTO>(service);
    }
}
