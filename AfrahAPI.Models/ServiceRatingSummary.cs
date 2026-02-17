using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمتل تقييم الخدمة بشكل ملخص
/// </summary>
public class ServiceRatingSummary
{


/// <summary>
/// معرف ملخص التقييم.
/// </summary>
[Key]
public Guid ServiceRatingSummaryId {get; set;}

    /// <summary>
    /// تقييم الخدمة بشكل متوسط
    /// </summary>
    public decimal RatingAverage { get; set; }
    /// <summary>
    /// عدد التقييمات
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




    

[ForeignKey("HallServices")]
public Guid HallServiceID { get; set; } 
public HallServices? HallServices { get; set; }


}
