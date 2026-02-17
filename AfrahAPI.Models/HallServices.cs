using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل الخدمات التي تقدمها الصالة
/// </summary>
public class HallServices
{

    /// <summary>
    /// معرف الخدمة
    /// </summary>
 [Key]
 public Guid   ServiceId { get; set; }
    /// <summary>
    /// اسم الخدمة
    /// </summary>
    [Required(ErrorMessage = "اسم الخدمة مطلوب")]
    [StringLength(200, ErrorMessage = "اسم الخدمة يجب ألا يتجاوز 200 حرف")]
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// وصف الخدمة
    /// </summary>
    [Required(ErrorMessage = "وصف الخدمة مطلوب")]
    [StringLength(1000, ErrorMessage = "الوصف يجب ألا يتجاوز 1000 حرف")]
public string Description { get; set; } = string.Empty;
    /// <summary>
    /// سعر الخدمة
    /// </summary>
 public decimal   Price { get; set; }
    /// <summary>
    /// هل الخدمة مشمولة في سعر الحجز الأساسي؟
    /// </summary>
    public bool IsIncluded { get; set; }

    /// <summary>
    /// هل يمكن للعميل طلب هذه الخدمة بشكل منفصل؟
    /// </summary>
    public bool   IsOptional { get; set; }

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


    public Guid? ServiceRatingSummaryID { get; set; }
    public ServiceRatingSummary? ServiceRatingSummary { get; set; }

    public ICollection<InvoiceItems> InvoiceItems { get; set; }=new List<InvoiceItems>();
    

    public ICollection<ServiceRating> ServiceRatings { get; set; }=new List<ServiceRating>();

   




}
