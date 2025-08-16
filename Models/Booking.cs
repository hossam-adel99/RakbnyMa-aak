using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Models
{
    public class Booking: BaseEntity
    {
        [Required(ErrorMessage = "المستخدم مطلوب.")]
        [Display(Name = "المستخدم")]
        public string UserId { get; set; } 

        [Required(ErrorMessage = "الرحلة مطلوبة.")]
        [Display(Name = "الرحلة")]
        public int TripId { get; set; }

        [Required]
        [Display(Name = "حالة الطلب")]
        public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;

        [Required(ErrorMessage = "يرجى إدخال عدد المقاعد.")]
        [Range(1, 150, ErrorMessage = "يجب أن يكون عدد المقاعد على الأقل 1.")]
        [Display(Name = "عدد المقاعد")]
        public int NumberOfSeats { get; set; }

        //public bool IsPaid { get; set; } =false;
        //public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "سعر المقعد مطلوب.")]
        [Range(1, double.MaxValue, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        [Display(Name = "سعر المقعد")]
        public decimal PricePerSeat { get; set; }

        public virtual Payment Payment { get; set; }

        [Required]
        [Display(Name = "حالة الدفع")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Update TotalPrice to be stored in DB
        [Display(Name = "السعر الإجمالي")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice => PricePerSeat * NumberOfSeats;

        [Display(Name = "هل بدأت الرحلة؟")]
        public bool HasStarted { get; set; } = false;

        [Display(Name = "هل انتهت الرحلة؟")]
        public bool HasEnded { get; set; } = false;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }
    }
}
