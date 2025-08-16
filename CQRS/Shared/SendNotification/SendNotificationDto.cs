using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Commands.SendNotification
{
    public class SendNotificationDto
    {
        [Required(ErrorMessage = "معرّف المستلم مطلوب.")]
        [Display(Name = "معرّف المستلم")]
        public string ReceiverId { get; set; }

        [Required(ErrorMessage = "معرّف المرسل مطلوب.")]
        [Display(Name = "معرّف المرسل")]
        public string SenderUserId { get; set; }

        [Required(ErrorMessage = "محتوى الرسالة مطلوب.")]
        [StringLength(500, ErrorMessage = "يجب ألا تتجاوز الرسالة 500 حرف.")]
        [Display(Name = "الرسالة")]
        public string Message { get; set; }
    }
}
