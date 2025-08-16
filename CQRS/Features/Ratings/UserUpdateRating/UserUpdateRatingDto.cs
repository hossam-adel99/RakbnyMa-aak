using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Ratings.UserUpdateRating
{
    public class UserUpdateRatingDto
    {
        [Required(ErrorMessage = "معرّف التقييم مطلوب.")]
        public int RatingId { get; set; }

        public string? RaterId { get; set; }

        [Range(1, 5, ErrorMessage = "قيمة التقييم يجب أن تكون بين 1 و 5.")]
        public int? RatingValue { get; set; }

        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز التعليق 500 حرف.")]
        public string? Comment { get; set; }
    }
}
