using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Ratings.DriverAddRating
{
    public class DriverAddRatingDto
    {
        [Required(ErrorMessage = "معرّف الرحلة مطلوب.")]
        public int TripId { get; set; }

        public string? RaterId { get; set; }

        [Required(ErrorMessage = "معرّف الراكب المراد تقييمه مطلوب.")]
        public string RatedPassengerId { get; set; }

        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز التعليق 500 حرف.")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "قيمة التقييم مطلوبة.")]
        [Range(1, 5, ErrorMessage = "يجب أن تكون قيمة التقييم بين 1 و 5.")]
        public int RatingValue { get; set; }
    }
}
