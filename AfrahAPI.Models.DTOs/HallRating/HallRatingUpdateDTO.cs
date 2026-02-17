using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallRating;

public class HallRatingUpdateDTO
{
    [Required(ErrorMessage = "معرف التقييم مطلوب")]
    public Guid RatingID { get; set; }

    [Required(ErrorMessage = "التقييم مطلوب")]
    [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5")]
    public int OverallRating { get; set; }

    [StringLength(2000, ErrorMessage = "التعليق يجب ألا يتجاوز 2000 حرف")]
    public string? Comment { get; set; }
}
