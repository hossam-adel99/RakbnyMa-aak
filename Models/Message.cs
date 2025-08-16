using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.Models
{
    public class Message : BaseEntity
    {
        [Required(ErrorMessage = "الرحلة مطلوبة.")]
        [Display(Name = "الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "المرسل مطلوب.")]
        [Display(Name = "المرسل")]
        public string SenderId { get; set; }

        [Required(ErrorMessage = "محتوى الرسالة مطلوب.")]
        [MaxLength(1000, ErrorMessage = "يجب ألا يزيد محتوى الرسالة عن 1000 حرف.")]
        [Display(Name = "محتوى الرسالة")]
        public string Content { get; set; }

        [Display(Name = "تاريخ الإرسال")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public virtual Trip Trip { get; set; }
        public virtual ApplicationUser Sender { get; set; }
    }
}
