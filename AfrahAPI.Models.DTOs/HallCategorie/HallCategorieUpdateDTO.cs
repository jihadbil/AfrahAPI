using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallCategorie;

/// <summary>
/// DTO لتحديث بيانات فئة صالة موجودة
/// </summary>
public class HallCategorieUpdateDTO
{
    /// <summary>
    /// معرف الفئة
    /// </summary>
    [Required(ErrorMessage = "معرف الفئة مطلوب")]
    public Guid CategoryID { get; set; }

    /// <summary>
    /// اسم الفئة
    /// </summary>
    [Required(ErrorMessage = "اسم الفئة مطلوب")]
    [StringLength(100, ErrorMessage = "اسم الفئة يجب ألا يتجاوز 100 حرف")]
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// وصف الفئة
    /// </summary>
    [Required(ErrorMessage = "الوصف مطلوب")]
    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    public string Description { get; set; } = string.Empty;
}
