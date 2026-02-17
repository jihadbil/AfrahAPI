using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل تقييم الصالة من قبل العملاء
/// </summary>
public class HallRating
{


    /// <summary>
    /// مُعرّف فريد لكل تقييم.
    /// </summary>
[Key]
    public Guid RatingID { get; set; }



    /// <summary>
    /// تقييم الصالة بشكل عام (من 1 إلى 5).
    /// </summary>
    public int OverallRating { get; set; }



    /// <summary>
    /// تعليق العميل على الصالة.
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

    public Guid HallID { get; set; }

    public Hall? Hall { get; set; }


    public Guid CustomerID { get; set; }

    public Customer? Customer { get; set; }

}
