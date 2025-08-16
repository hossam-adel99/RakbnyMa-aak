using System.ComponentModel.DataAnnotations;
using static RakbnyMa_aak.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RakbnyMa_aak.Models
{
    public class Driver
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "تم الحذف")]
        [Required]
        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        [Display(Name = "إصدار الصف")]
        public byte[]? RowVersion { get; set; }

        [Key]
        [ForeignKey("ApplicationUser")] // UserId is PK and FK to ApplicationUser
        public string UserId { get; set; }

        [Required(ErrorMessage = "رقم البطاقة مطلوب.")]
        [RegularExpression(@"^[2-3]\d{13}$", ErrorMessage = "يجب أن يتكون رقم البطاقة من 14 رقمًا ويبدأ بـ 2 أو 3.")]
        [Display(Name = "الرقم القومي")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "نوع السيارة مطلوب.")]
        [Display(Name = "نوع السيارة")]
        public CarType CarType { get; set; }

        [Required(ErrorMessage = "موديل السيارة مطلوب. مثل (Kia Cerato 2023)")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز موديل السيارة 50 حرفًا. مثل (Kia Cerato 2023)")]
        [Display(Name = "موديل السيارة")]
        public string CarModel { get; set; }

        [Required(ErrorMessage = "لون السيارة مطلوب.")]
        [Display(Name = "لون السيارة")]
        public CarColor CarColor { get; set; }

        [Required(ErrorMessage = "سعة السيارة مطلوبة.")]
        [Range(2, 150, ErrorMessage = "يجب أن تكون سعة السيارة على الأقل 2.")]
        [Display(Name = "سعة السيارة")]
        public int CarCapacity { get; set; }

        [Required(ErrorMessage = "رقم لوحة السيارة مطلوب.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "يجب أن يتراوح رقم لوحة السيارة بين 2 و10 أحرف.")]
        public string CarPlateNumber { get; set; }

        [Required(ErrorMessage = "صورة البطاقة مطلوبة.")]
        [Display(Name = "صورة الرقم القومي")]
        public string DriverNationalIdImagePath { get; set; }

        [Required(ErrorMessage = "صورة رخصة القيادة مطلوبة.")]
        [Display(Name = "صورة رخصة القيادة")]
        public string DriverLicenseImagePath { get; set; }

        [Required(ErrorMessage = "صورة رخصة السيارة مطلوبة.")]
        [Display(Name = "صورة رخصة السيارة")]
        public string CarLicenseImagePath { get; set; }

        [Required(ErrorMessage = "الصورة الشخصية مطلوبة.")]
        [Display(Name = "صورة شخصية")]
        public string SelfieImagePath { get; set; }

        [Display(Name = "تم التحقق من الوجه")]
        public bool IsFaceVerified { get; set; } = false;

        [Display(Name = "تم التحقق من الهاتف")]
        public bool IsPhoneVerified { get; set; } = false;

        [Display(Name = "تمت الموافقة")]
        public bool IsApproved { get; set; } = false; // For admin approval

        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "رمز التحقق من الهاتف")]
        public string? PhoneVerificationCode { get; set; }

        // Navigation property back to ApplicationUser
        public virtual ApplicationUser User { get; set; }

        // Driver has many Trips
        public virtual ICollection<Trip>? Trips { get; set; }

        public Driver()
        {
            Trips = new HashSet<Trip>();
        }
    }
}
