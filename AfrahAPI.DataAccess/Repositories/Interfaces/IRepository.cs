using System.Linq.Expressions;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة عامة لعمليات قاعدة البيانات الأساسية (Generic Repository Pattern)
/// </summary>
/// <typeparam name="T">نوع الكيان الذي سيتم العمل عليه</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// جلب عنصر بواسطة المعرف
    /// </summary>
    /// <typeparam name="TKey">نوع المعرف (Guid, int, etc.)</typeparam>
    /// <param name="id">المعرف الفريد للعنصر</param>
    /// <returns>العنصر المطلوب أو null إذا لم يتم العثور عليه</returns>
    Task<T?> GetByIdAsync<TKey>(TKey id) where TKey : notnull;

    /// <summary>
    /// جلب جميع العناصر من قاعدة البيانات
    /// </summary>
    /// <returns>قائمة بجميع العناصر</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// البحث عن العناصر التي تطابق الشرط المعطى
    /// </summary>
    /// <param name="predicate">تعبير Lambda للبحث</param>
    /// <returns>قائمة بالعناصر المطابقة للشرط</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// البحث عن أول عنصر يطابق الشرط المعطى
    /// </summary>
    /// <param name="predicate">تعبير Lambda للبحث</param>
    /// <returns>أول عنصر مطابق أو null</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// إضافة عنصر جديد إلى قاعدة البيانات
    /// </summary>
    /// <param name="entity">العنصر المراد إضافته</param>
    /// <returns>العنصر المضاف</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// إضافة مجموعة من العناصر إلى قاعدة البيانات
    /// </summary>
    /// <param name="entities">مجموعة العناصر المراد إضافتها</param>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// تحديث عنصر موجود في قاعدة البيانات
    /// </summary>
    /// <param name="entity">العنصر المراد تحديثه</param>
    void Update(T entity);

    /// <summary>
    /// تحديث مجموعة من العناصر في قاعدة البيانات
    /// </summary>
    /// <param name="entities">مجموعة العناصر المراد تحديثها</param>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// حذف عنصر من قاعدة البيانات
    /// </summary>
    /// <param name="entity">العنصر المراد حذفه</param>
    void Remove(T entity);

    /// <summary>
    /// حذف مجموعة من العناصر من قاعدة البيانات
    /// </summary>
    /// <param name="entities">مجموعة العناصر المراد حذفها</param>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// عد إجمالي العناصر في قاعدة البيانات
    /// </summary>
    /// <returns>عدد العناصر</returns>
    Task<int> CountAsync();

    /// <summary>
    /// عد العناصر التي تطابق الشرط المعطى
    /// </summary>
    /// <param name="predicate">تعبير Lambda للبحث</param>
    /// <returns>عدد العناصر المطابقة</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// التحقق من وجود عنصر واحد على الأقل يطابق الشرط المعطى
    /// </summary>
    /// <param name="predicate">تعبير Lambda للبحث</param>
    /// <returns>true إذا وجد عنصر مطابق، false خلاف ذلك</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// التحقق من وجود أي عناصر في قاعدة البيانات
    /// </summary>
    /// <returns>true إذا وجدت عناصر، false خلاف ذلك</returns>
    Task<bool> AnyAsync();
}
