using System;
using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Ratings.DriverGetRatings
{
    public class DriverRatingDto
    {
        public int RatingId { get; set; }

        [Required(ErrorMessage = "معرّف الرحلة مطلوب.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "قيمة التقييم مطلوبة.")]
        [Range(1, 5, ErrorMessage = "يجب أن يكون التقييم بين 1 و 5.")]
        public int RatingValue { get; set; }

        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز التعليق 500 حرف.")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "اسم المقيم مطلوب.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز اسم المقيم 100 حرف.")]
        public string RaterName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
