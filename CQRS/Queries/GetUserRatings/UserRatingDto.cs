using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Queries.GetUserRatings
{
    public class UserRatingDto
    {
        [Required]
        public int RatingId { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required(ErrorMessage = "اسم السائق مطلوب.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز اسم السائق 100 حرف.")]
        public string DriverName { get; set; }

        [Range(1, 5, ErrorMessage = "يجب أن تكون القيمة بين 1 و 5.")]
        public int RatingValue { get; set; }

        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز التعليق 500 حرف.")]
        public string? Comment { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
