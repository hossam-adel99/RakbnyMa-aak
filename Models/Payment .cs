using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Models
{
    public class Payment : BaseEntity
    {
        [Required(ErrorMessage = "المستخدم مطلوب.")]
        [Display(Name = "المستخدم")]
        public string UserId { get; set; }

        [Display(Name = "الحجز")]
        public int? BookingId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من 0.")]
        [Display(Name = "المبلغ")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "طريقة الدفع مطلوبة.")]
        [Display(Name = "طريقة الدفع")]
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "حالة الدفع مطلوبة.")]
        [Display(Name = "حالة الدفع")]
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [Required(ErrorMessage = "نوع الدفع مطلوب.")]
        [Display(Name = "نوع الدفع")]
        [EnumDataType(typeof(PaymentType))]
        public PaymentType PaymentType { get; set; }

        [Display(Name = "رقم المعاملة")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز رقم المعاملة 100 حرف.")]
        public string? TransactionId { get; set; }

        [Display(Name = "تاريخ الدفع")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "سبب الفشل")]
        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز سبب الفشل 500 حرف.")]
        public string? FailureReason { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
    }
}
