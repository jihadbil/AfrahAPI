using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل تقييم الخدمة من قبل العملاء
/// </summary>
public class ServiceRating
{
    /// <summary>
    /// يمتل معرف تقييم الخدمة
    /// </summary>
   [Key]
    public Guid ServiceRatingID { get; set; }
    /// <summary>
    /// يمتل تقييم الخدمة   
    /// </summary>
    public int Rating { get; set; }
    /// <summary>
    /// يمثل تعليق العميل على الخدمة
    /// </summary>
    [StringLength(2000, ErrorMessage = "التعليق يجب ألا يتجاوز 2000 حرف")]
    public string? Comment { get; set; }

    /// ///////////////حقول الوقت//////////

    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public Guid HallServiceID { get; set; }
    public HallServices? HallServices { get; set; }

    public Guid CustomerID { get; set; }
    public Customer? Customer { get; set; }
}
