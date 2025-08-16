using RakbnyMa_aak.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.DriverDTOs.UpdateProfileDTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "كلمة المرور القديمة مطلوبة.")]
        [MinLength(6, ErrorMessage = "يجب أن تكون كلمة المرور مكونة من 6 أحرف على الأقل.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
          ErrorMessage = "كلمة المرور غير صحيحة.")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور القديمة")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [MinLength(6, ErrorMessage = "يجب أن تكون كلمة المرور مكونة من 6 أحرف على الأقل.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
          ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف كبير وحرف صغير ورقم وحرف خاص واحد على الأقل.")]
        [DataType(DataType.Password)]
        [NotEqualTo("OldPassword", ErrorMessage = "يجب أن تكون كلمة المرور الجديدة مختلفة عن القديمة.")]
        [Display(Name = "كلمة المرور الجديدة")]
        public string NewPassword { get; set; }

        [Display(Name = "تأكيد كلمة المرور")]
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمتا المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; }
    }
}
