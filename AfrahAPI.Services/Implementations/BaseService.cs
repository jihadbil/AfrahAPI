using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق الخدمة الأساسية العامة - يوفر العمليات الأساسية لجميع الخدمات
/// </summary>
/// <typeparam name="TEntity">نوع الكيان</typeparam>
/// <typeparam name="TCreateDTO">DTO الإنشاء</typeparam>
/// <typeparam name="TReadDTO">DTO القراءة</typeparam>
/// <typeparam name="TUpdateDTO">DTO التحديث</typeparam>
public abstract class BaseService<TEntity, TCreateDTO, TReadDTO, TUpdateDTO> 
    : IBaseService<TEntity, TCreateDTO, TReadDTO, TUpdateDTO>
    where TEntity : class
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IRepository<TEntity> _repository;

    /// <summary>
    /// المُنشئ الأساسي للخدمة
    /// </summary>
    /// <param name="unitOfWork">وحدة العمل</param>
    /// <param name="mapper">AutoMapper</param>
    /// <param name="repository">المستودع الخاص بالكيان</param>
    protected BaseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repository = repository;
    }

    /// <summary>
    /// الحصول على عنصر بواسطة معرفه الفريد
    /// </summary>
    public virtual async Task<TReadDTO?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity != null ? _mapper.Map<TReadDTO>(entity) : default;
    }

    /// <summary>
    /// الحصول على جميع العناصر
    /// </summary>
    public virtual async Task<IEnumerable<TReadDTO>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TReadDTO>>(entities);
    }

    /// <summary>
    /// إنشاء عنصر جديد
    /// </summary>
    public virtual async Task<TReadDTO> CreateAsync(TCreateDTO createDto)
    {
        var entity = _mapper.Map<TEntity>(createDto);
        await _repository.AddAsync(entity);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<TReadDTO>(entity);
    }

    /// <summary>
    /// تحديث عنصر موجود
    /// </summary>
    public virtual async Task<TReadDTO?> UpdateAsync(Guid id, TUpdateDTO updateDto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return default;

        _mapper.Map(updateDto, entity);
        _repository.Update(entity);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<TReadDTO>(entity);
    }

    /// <summary>
    /// حذف عنصر
    /// </summary>
    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return false;

        _repository.Remove(entity);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    /// <summary>
    /// التحقق من وجود عنصر
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity != null;
    }

    /// <summary>
    /// عد جميع العناصر
    /// </summary>
    public virtual async Task<int> CountAsync()
    {
        return await _repository.CountAsync();
    }
}
