using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.UserDTOs.UpdateProfileDTOs
{
    public class UpdateUserProfileDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        [Display(Name = "الاسم الكامل")]
        [MinLength(3, ErrorMessage = "يجب أن لا يقل الاسم الكامل عن 3 أحرف.")]
        [MaxLength(100, ErrorMessage = "يجب أن لا يزيد الاسم الكامل عن 100 حرف.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",
            ErrorMessage = "يجب أن يتكون رقم الهاتف من 11 رقمًا ويبدأ بـ 010 أو 011 أو 012 أو 015.")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public IFormFile? Picture { get; set; }
    }
}
