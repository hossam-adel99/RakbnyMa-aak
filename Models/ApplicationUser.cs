using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "الاسم الكامل")]
        [Required(ErrorMessage = "يرجى إدخال الاسم الكامل.")]
        [MinLength(3, ErrorMessage = "يجب ألا يقل الاسم الكامل عن 3 أحرف.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يزيد الاسم الكامل عن 100 حرف.")]
        public string FullName { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public string Picture { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "يجب أن يكون رقم الهاتف 11 رقمًا ويبدأ بـ 010 أو 011 أو 012 أو 015.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "نوع المستخدم")]
        [Required(ErrorMessage = "يرجى اختيار نوع المستخدم.")]
        [EnumDataType(typeof(UserType))]
        [Column(TypeName = "nvarchar(20)")]
        public UserType UserType { get; set; }

        [Display(Name = "النوع")]
        [Required(ErrorMessage = "يرجى اختيار النوع.")]
        [EnumDataType(typeof(Gender))]
        [Column(TypeName = "nvarchar(20)")]
        public Gender Gender { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "تاريخ التعديل")]
        public DateTime? UpdatedAt { get; set; }
        public virtual Driver? Driver { get; set; }
        public virtual ICollection<Rating>? RatingsGiven { get; set; }
        public virtual ICollection<Rating>? RatingsReceived { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }

        // User can send many messages
        public virtual ICollection<Message>? SentMessages { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }

        public virtual Wallet Wallet { get; set; }

        public ApplicationUser()
        {
            RatingsGiven = new HashSet<Rating>();
            RatingsReceived = new HashSet<Rating>();
            Bookings = new HashSet<Booking>();
            SentMessages = new HashSet<Message>();
            Notifications = new HashSet<Notification>();
        }

    }
}
