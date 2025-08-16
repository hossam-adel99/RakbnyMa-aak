using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Ratings.CommonAddRating
{
    public class CommonAddRatingDto
    {
        [Required(ErrorMessage = "رقم الرحلة مطلوب.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "رقم المُقيِّم مطلوب.")]
        public string RaterId { get; set; }

        [Required(ErrorMessage = "رقم المُقَيَّم مطلوب.")]
        public string RatedId { get; set; }

        [Required(ErrorMessage = "قيمة التقييم مطلوبة.")]
        [Range(1, 5, ErrorMessage = "يجب أن تكون قيمة التقييم بين 1 و 5.")]
        public int RatingValue { get; set; }

        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز التعليق 500 حرف.")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "يرجى تحديد نوع التقييم.")]
        [Display(Name = "هل التقييم للسائق؟")]
        public bool IsDriverRating { get; set; } // True: driver rating passenger | False: passenger rating driver
    }
}
