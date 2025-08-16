using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.DTOs.Auth.RequestDTOs
{
    public class RegisterUserRequestDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        [Display(Name = "الاسم الكامل")]
        [MinLength(3, ErrorMessage = "يجب ألا يقل الاسم الكامل عن 3 أحرف.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم الكامل 100 حرف.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "عنوان البريد الإلكتروني غير صالح.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [MinLength(6, ErrorMessage = "يجب ألا تقل كلمة المرور عن 6 أحرف.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف كبير، وحرف صغير، ورقم، وحرف خاص واحد على الأقل.")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب.")]
        [Compare(nameof(Password), ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين.")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",
            ErrorMessage = "يجب أن يتكون رقم الهاتف من 11 رقماً ويبدأ بـ 010 أو 011 أو 012 أو 015.")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Display(Name = "الجنس")]
        [Required(ErrorMessage = "يرجى اختيار الجنس.")]
        [EnumDataType(typeof(Gender))]
        [Column(TypeName = "nvarchar(20)")]
        public Gender Gender { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public IFormFile? Picture { get; set; }
    }
}
