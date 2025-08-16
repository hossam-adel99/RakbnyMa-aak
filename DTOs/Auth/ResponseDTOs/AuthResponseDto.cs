using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.Auth.Responses
{
    public class AuthResponseDto
    {
        [Required(ErrorMessage = "معرّف المستخدم مطلوب.")]
        [Display(Name = "معرّف المستخدم")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "الرمز المميز مطلوب.")]
        [Display(Name = "الرمز المميز (JWT)")]
        public string Token { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "الدور مطلوب.")]
        [Display(Name = "دور المستخدم")]
        public string Role { get; set; }
    }
}
