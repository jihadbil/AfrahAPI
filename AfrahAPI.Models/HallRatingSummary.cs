using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AfrahAPI.Models;
/// <<summary>
/// يمثل ملخص تقييمات القاعة.
/// </summary>
public class HallRatingSummary
{

/// <summary>
/// معرف ملخص التقييم.
/// </summary>
[Key]
public Guid HallRatingSummaryId {get; set;}
    /// <summary>
    /// متوسط التقييم العام.
    /// </summary>
    public decimal OverallRatingAverage { get; set; }

    /// <summary>
    /// إجمالي عدد التقييمات.
    /// </summary>
    public int TotalRatingsCount { get; set; }









    /// ///////////////حقول الوقت//////////


    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }


[ForeignKey("Hall")]
public Guid HallID { get; set; } 
public Hall? Hall { get; set; }

}
