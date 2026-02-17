using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallMedia;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة وسائط الصالات
/// </summary>
public class HallMediaService : BaseService<HallMedia, HallMediaCreateDTO, HallMediaReadDTO, HallMediaUpdateDTO>, IHallMediaService
{
    private readonly IHallMediaRepository _hallMediaRepository;
    private readonly IHallRepository _hallRepository;

    public HallMediaService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallMediaRepository hallMediaRepository,
        IHallRepository hallRepository)
        : base(unitOfWork, mapper, hallMediaRepository)
    {
        _hallMediaRepository = hallMediaRepository;
        _hallRepository = hallRepository;
    }

    public async Task<IEnumerable<HallMediaReadDTO>> GetMediaByHallIdAsync(Guid hallId)
    {
        var media = await _hallMediaRepository.FindAsync(m => m.HallID == hallId);
        return _mapper.Map<IEnumerable<HallMediaReadDTO>>(media);
    }

    public async Task<HallMediaReadDTO> UploadMediaAsync(Guid hallId, HallMediaCreateDTO createDto)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null)
            throw new Exception("الصالة غير موجودة");

        var media = _mapper.Map<HallMedia>(createDto);
        media.MediaID = Guid.NewGuid();
        media.HallID = hallId;
        media.CreatedAt = DateTime.UtcNow;

        await _hallMediaRepository.AddAsync(media);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallMediaReadDTO>(media);
    }

    public async Task<bool> DeleteMediaAsync(Guid mediaId)
    {
        var media = await _hallMediaRepository.GetByIdAsync(mediaId);
        if (media == null)
            return false;

        _hallMediaRepository.Remove(media);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public override async Task<HallMediaReadDTO> CreateAsync(HallMediaCreateDTO createDto)
    {
        var media = _mapper.Map<HallMedia>(createDto);
        media.MediaID = Guid.NewGuid();
        media.CreatedAt = DateTime.UtcNow;

        await _hallMediaRepository.AddAsync(media);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallMediaReadDTO>(media);
    }
}
