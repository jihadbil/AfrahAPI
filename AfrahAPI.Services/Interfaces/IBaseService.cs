namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة الخدمة الأساسية العامة - تحتوي على العمليات الأساسية لجميع الخدمات
/// </summary>
/// <typeparam name="TEntity">نوع الكيان</typeparam>
/// <typeparam name="TCreateDTO">DTO الإنشاء</typeparam>
/// <typeparam name="TReadDTO">DTO القراءة</typeparam>
/// <typeparam name="TUpdateDTO">DTO التحديث</typeparam>
public interface IBaseService<TEntity, TCreateDTO, TReadDTO, TUpdateDTO> 
    where TEntity : class
{
    /// <summary>
    /// الحصول على عنصر بواسطة معرفه الفريد
    /// </summary>
    /// <param name="id">المعرف الفريد</param>
    /// <returns>DTO القراءة أو null إذا لم يتم العثور عليه</returns>
    Task<TReadDTO?> GetByIdAsync(Guid id);

    /// <summary>
    /// الحصول على جميع العناصر
    /// </summary>
    /// <returns>قائمة من DTOs القراءة</returns>
    Task<IEnumerable<TReadDTO>> GetAllAsync();

    /// <summary>
    /// إنشاء عنصر جديد
    /// </summary>
    /// <param name="createDto">DTO الإنشاء</param>
    /// <returns>DTO القراءة للعنصر المُنشأ</returns>
    Task<TReadDTO> CreateAsync(TCreateDTO createDto);

    /// <summary>
    /// تحديث عنصر موجود
    /// </summary>
    /// <param name="id">معرف العنصر المراد تحديثه</param>
    /// <param name="updateDto">DTO التحديث</param>
    /// <returns>DTO القراءة للعنصر المُحدث</returns>
    Task<TReadDTO?> UpdateAsync(Guid id, TUpdateDTO updateDto);

    /// <summary>
    /// حذف عنصر
    /// </summary>
    /// <param name="id">معرف العنصر المراد حذفه</param>
    /// <returns>true إذا تم الحذف بنجاح، false خلاف ذلك</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// التحقق من وجود عنصر بمعرف معين
    /// </summary>
    /// <param name="id">المعرف المراد البحث عنه</param>
    /// <returns>true إذا كان العنصر موجود، false خلاف ذلك</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    /// عد جميع العناصر
    /// </summary>
    /// <returns>عدد العناصر</returns>
    Task<int> CountAsync();
}
