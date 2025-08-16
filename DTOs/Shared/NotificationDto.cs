using System.ComponentModel.DataAnnotations;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.DTOs.Shared
{
    public class NotificationDto
    {
        [Required(ErrorMessage = "الرسالة مطلوبة.")]
        [MaxLength(500, ErrorMessage = "يجب ألا تتجاوز الرسالة 500 حرف.")]
        [Display(Name = "نص الإشعار")]
        public string Message { get; set; }

        [Required(ErrorMessage = "معرّف المرسل مطلوب.")]
        [Display(Name = "معرّف المرسل")]
        public string SenderId { get; set; }

        [Required(ErrorMessage = "الاسم الكامل للمرسل مطلوب.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم الكامل للمرسل 100 حرف.")]
        [Display(Name = "الاسم الكامل للمرسل")]
        public string SenderFullName { get; set; }

        [Required(ErrorMessage = "رابط صورة المرسل مطلوب.")]
        [Url(ErrorMessage = "تنسيق رابط صورة المرسل غير صالح.")]
        [Display(Name = "رابط صورة المرسل")]
        public string SenderPicture { get; set; }

        [Display(Name = "نوع الإشعار")]
        public NotificationType Type { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        [Display(Name = "معرّف الكيان المرتبط")]
        public string? RelatedEntityId { get; set; }
    }
}
