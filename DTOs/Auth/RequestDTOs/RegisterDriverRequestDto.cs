using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.DTOs.Auth.RequestDTOs
{
    public class RegisterDriverRequestDto
    {
        [Display(Name = "الاسم الكامل")]
        [Required(ErrorMessage = "يرجى إدخال الاسم الكامل.")]
        [MinLength(3, ErrorMessage = "يجب ألا يقل الاسم الكامل عن 3 أحرف.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم الكامل 100 حرف.")]
        public string FullName { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [MinLength(6, ErrorMessage = "يجب ألا تقل كلمة المرور عن 6 أحرف.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
           ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف كبير، حرف صغير، رقم، وحرف خاص على الأقل.")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "تأكيد كلمة المرور")]
        [Required(ErrorMessage = "يرجى تأكيد كلمة المرور.")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$", ErrorMessage = "يجب أن يتكون رقم الهاتف من 11 رقمًا ويبدأ بـ 010 أو 011 أو 012 أو 015.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "النوع")]
        [Required(ErrorMessage = "يرجى اختيار النوع.")]
        [EnumDataType(typeof(Gender))]
        [Column(TypeName = "nvarchar(20)")]
        public Gender Gender { get; set; }

        [Display(Name = "الرقم القومي")]
        [Required(ErrorMessage = "الرقم القومي مطلوب.")]
        [RegularExpression(@"^[23]\d{13}$", ErrorMessage = "يجب أن يبدأ الرقم القومي بـ 2 أو 3 ويتكون من 14 رقمًا.")]
        public string NationalId { get; set; }

        [Display(Name = "نوع السيارة")]
        [Required(ErrorMessage = "نوع السيارة مطلوب.")]
        public CarType CarType { get; set; }

        [Required(ErrorMessage = "موديل السيارة مطلوب. مثال: Kia Cerato 2023")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز موديل السيارة 50 حرفًا. مثال: Kia Cerato 2023")]
        [Display(Name = "موديل السيارة")]
        public string CarModel { get; set; }

        [Required(ErrorMessage = "لون السيارة مطلوب.")]
        [Display(Name = "لون السيارة")]
        public CarColor CarColor { get; set; }

        [Display(Name = "سعة السيارة")]
        [Required(ErrorMessage = "سعة السيارة مطلوبة.")]
        [Range(2, 150, ErrorMessage = "يجب أن تكون سعة السيارة بين 2 و150.")]
        public int CarCapacity { get; set; }

        [Required(ErrorMessage = "رقم لوحة السيارة مطلوب.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "يجب أن يتراوح رقم لوحة السيارة بين 2 و10 أحرف.")]
        public string CarPlateNumber { get; set; }

        [Display(Name = "صورة البطاقة الشخصية")]
        [Required(ErrorMessage = "صورة البطاقة الشخصية مطلوبة.")]
        public IFormFile NationalIdImage { get; set; }

        [Display(Name = "صورة رخصة القيادة")]
        [Required(ErrorMessage = "صورة رخصة القيادة مطلوبة.")]
        public IFormFile DriverLicenseImage { get; set; }

        [Display(Name = "صورة رخصة السيارة")]
        [Required(ErrorMessage = "صورة رخصة السيارة مطلوبة.")]
        public IFormFile CarLicenseImage { get; set; }

        [Display(Name = "صورة شخصية (سيلفي)")]
        [Required(ErrorMessage = "الصورة الشخصية (سيلفي) مطلوبة.")]
        public IFormFile SelfieImage { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public IFormFile? Picture { get; set; }
    }
}
