using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Models
{
    public class Notification : BaseEntity
    {
        [Required(ErrorMessage = "معرف المستخدم مطلوب.")]
        [Display(Name = "معرف المستخدم")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "رسالة الإشعار مطلوبة.")]
        [MaxLength(500, ErrorMessage = "يجب ألا تتجاوز رسالة الإشعار 500 حرف.")]
        [Display(Name = "الرسالة")]
        public string Message { get; set; }

        [Display(Name = "تمت قراءتها")]
        public bool IsRead { get; set; } = false;

        public NotificationType Type { get; set; } = NotificationType.Custom;

        public string? RelatedEntityId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "المستخدم")]
        public virtual ApplicationUser User { get; set; }
    }
}
