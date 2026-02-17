using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.ServiceRating;

public class ServiceRatingCreateDTO
{
    [Required(ErrorMessage = "التقييم مطلوب")]
    [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5")]
    public int Rating { get; set; }

    [StringLength(2000, ErrorMessage = "التعليق يجب ألا يتجاوز 2000 حرف")]
    public string? Comment { get; set; }

    [Required(ErrorMessage = "معرف الخدمة مطلوب")]
    public Guid HallServiceID { get; set; }

    [Required(ErrorMessage = "معرف العميل مطلوب")]
    public Guid CustomerID { get; set; }
}
