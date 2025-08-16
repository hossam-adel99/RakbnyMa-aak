using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Ratings.DriverUpdateRating
{
    public class DriverUpdateRatingDto
    {
        [Required(ErrorMessage = "يجب إدخال رقم التقييم.")]
        public int RatingId { get; set; }

        public string? RaterId { get; set; }

        [Range(1, 5, ErrorMessage = "يجب أن تكون قيمة التقييم بين 1 و 5.")]
        public int? RatingValue { get; set; }

        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز التعليق 500 حرف.")]
        public string? Comment { get; set; }
    }
}
