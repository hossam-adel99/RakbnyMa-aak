using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.DriverDTOs.UpdateProfileDTOs
{
    public class UpdateDriverProfileDto
    {
        [Display(Name = "الاسم الكامل")]
        [Required(ErrorMessage = "يرجى إدخال الاسم الكامل.")]
        [MinLength(3, ErrorMessage = "يجب ألا يقل الاسم الكامل عن 3 أحرف.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يزيد الاسم الكامل عن 100 حرف.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني.")]
        [EmailAddress(ErrorMessage = "يرجى إدخال بريد إلكتروني صالح.")]
        public string Email { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "يجب أن يكون رقم الهاتف 11 رقمًا ويبدأ بـ 010 أو 011 أو 012 أو 015.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "صورة السيلفي")]
        [Required(ErrorMessage = "صورة السيلفي مطلوبة.")]
        public IFormFile SelfieImage { get; set; }

        [Display(Name = "صورة بطاقة الرقم القومي")]
        [Required(ErrorMessage = "صورة بطاقة الرقم القومي مطلوبة.")]
        public IFormFile NationalIdImage { get; set; }
    }
}
