using System;

namespace AfrahAPI.Models.DTOs.Common;

/// <summary>
/// DTO للترقيم (Pagination) يستخدم في قوائم البيانات
/// </summary>
public class PaginationDTO
{
    /// <summary>
    /// رقم الصفحة الحالية (يبدأ من 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// عدد العناصر في الصفحة الواحدة
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// إجمالي عدد العناصر
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// إجمالي عدد الصفحات
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// هل توجد صفحة سابقة؟
    /// </summary>
    public bool HasPrevious => PageNumber > 1;

    /// <summary>
    /// هل توجد صفحة تالية؟
    /// </summary>
    public bool HasNext => PageNumber < TotalPages;
}
