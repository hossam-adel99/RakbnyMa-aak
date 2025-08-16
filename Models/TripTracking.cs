using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RakbnyMa_aak.Models
{
    public class TripTracking : BaseEntity
    {
        [Required(ErrorMessage = "معرف الرحلة مطلوب.")]
        [Display(Name = "الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "خط العرض الحالي مطلوب.")]
        [Range(-90, 90, ErrorMessage = "يجب أن يكون خط العرض بين -90 و 90.")]
        [Display(Name = "خط العرض الحالي")]
        public double CurrentLat { get; set; }

        [Required(ErrorMessage = "خط الطول الحالي مطلوب.")]
        [Range(-180, 180, ErrorMessage = "يجب أن يكون خط الطول بين -180 و 180.")]
        [Display(Name = "خط الطول الحالي")]
        public double CurrentLong { get; set; }

        [Required(ErrorMessage = "الطابع الزمني مطلوب.")]
        [Display(Name = "الطابع الزمني")]
        public DateTime Timestamp { get; set; }

        [ForeignKey("TripId")]
        [Display(Name = "الرحلة")]
        public virtual Trip Trip { get; set; }
    }
}
