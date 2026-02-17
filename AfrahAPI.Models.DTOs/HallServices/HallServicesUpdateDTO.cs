using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallServices;

public class HallServicesUpdateDTO
{
    [Required(ErrorMessage = "معرف الخدمة مطلوب")]
    public Guid ServiceId { get; set; }

    [Required(ErrorMessage = "اسم الخدمة مطلوب")]
    [StringLength(200, ErrorMessage = "اسم الخدمة يجب ألا يتجاوز 200 حرف")]
    public string ServiceName { get; set; } = string.Empty;

    [Required(ErrorMessage = "وصف الخدمة مطلوب")]
    [StringLength(1000, ErrorMessage = "الوصف يجب ألا يتجاوز 1000 حرف")]
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public bool IsIncluded { get; set; }
    public bool IsOptional { get; set; }
}
