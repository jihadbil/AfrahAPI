using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallCategorie;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة فئات الصالات
/// </summary>
public class HallCategorieService : BaseService<HallCategorie, HallCategorieCreateDTO, HallCategorieReadDTO, HallCategorieUpdateDTO>, IHallCategorieService
{
    private readonly IHallCategorieRepository _categorieRepository;

    public HallCategorieService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallCategorieRepository categorieRepository)
        : base(unitOfWork, mapper, categorieRepository)
    {
        _categorieRepository = categorieRepository;
    }

    public async Task<HallCategorieReadDTO?> GetCategoryWithHallsAsync(Guid categoryId)
    {
        var category = await _categorieRepository.GetByIdAsync(categoryId);
        return category != null ? _mapper.Map<HallCategorieReadDTO>(category) : null;
    }

    public override async Task<HallCategorieReadDTO> CreateAsync(HallCategorieCreateDTO createDto)
    {
        var category = _mapper.Map<HallCategorie>(createDto);
        category.CategoryID = Guid.NewGuid();

        await _categorieRepository.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallCategorieReadDTO>(category);
    }
}
