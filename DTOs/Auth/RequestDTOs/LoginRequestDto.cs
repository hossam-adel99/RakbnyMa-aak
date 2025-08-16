using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.Auth.Requests
{
    public class LoginRequestDto
    {
        [Display(Name = "البريد الإلكتروني أو اسم المستخدم")]
        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني أو اسم المستخدم.")]
        public string EmailOrUsername { get; set; }

        [Display(Name = "كلمة المرور")]
        [Required(ErrorMessage = "يرجى إدخال كلمة المرور.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
