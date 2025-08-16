using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RakbnyMa_aak.Models
{
    public class Rating : BaseEntity
    {
        // Id is inherited from BaseEntity (int PK)
        [Required(ErrorMessage = "رقم الرحلة مطلوب.")]
        [Display(Name = "الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "رقم المقيّم مطلوب.")]
        [Display(Name = "المقيّم")]
        public string RaterId { get; set; }

        [Required(ErrorMessage = "رقم المُقيّم عليه مطلوب.")]
        [Display(Name = "المُقيّم عليه")]
        public string RatedId { get; set; }

        [Required(ErrorMessage = "قيمة التقييم مطلوبة.")]
        [Range(1, 5, ErrorMessage = "يجب أن يكون التقييم بين 1 و 5.")]
        [Display(Name = "قيمة التقييم")]
        public int RatingValue { get; set; }

        [MaxLength(1000, ErrorMessage = "يجب ألا يتجاوز التعليق 1000 حرف.")]
        [Display(Name = "التعليق")]
        public string Comment { get; set; }

        // Navigation properties
        [ForeignKey("TripId")]
        [Display(Name = "الرحلة")]
        public virtual Trip Trip { get; set; }

        [ForeignKey("RaterId")]
        [Display(Name = "المقيّم")]
        public virtual ApplicationUser Rater { get; set; }

        [ForeignKey("RatedId")]
        [Display(Name = "المُقيّم عليه")]
        public virtual ApplicationUser Rated { get; set; }

        // Business Logic Constraint Support:
        // This model structure supports the constraint. The actual enforcement
        // (checking if RaterId and RatedId were part of TripId) will happen in the
        // Business Logic Layer (e.g., a RatingService) before saving the rating.
        // You would query the Trip's bookings and driver to verify participation.
    }
}
