using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Ratings.UserAddRating
{
    public class UserAddRatingDto
    {
        [Required(ErrorMessage = "معرّف الرحلة مطلوب.")]
        public int TripId { get; set; }

        public string? RaterId { get; set; }

        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز التعليق 500 حرف.")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "قيمة التقييم مطلوبة.")]
        [Range(1, 5, ErrorMessage = "يجب أن تكون قيمة التقييم بين 1 و 5.")]
        public int RatingValue { get; set; }
    }
}
