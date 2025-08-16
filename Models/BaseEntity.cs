using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.Models
{
    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "تاريخ التعديل")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "تم الحذف")]
        [Required]
        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        [Display(Name = "إصدار السطر")]
        public byte[]? RowVersion { get; set; }
    }
}
