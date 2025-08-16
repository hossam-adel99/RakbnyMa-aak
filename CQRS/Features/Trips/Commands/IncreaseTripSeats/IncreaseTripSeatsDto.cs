using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.IncreaseTripSeats
{
    public class IncreaseTripSeatsDto
    {
        [Required(ErrorMessage = "معرف الرحلة مطلوب.")]
        [Display(Name = "معرف الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "عدد المقاعد مطلوب.")]
        [Range(1, 150, ErrorMessage = "يجب أن يكون عدد المقاعد على الأقل 1.")]
        [Display(Name = "عدد المقاعد المطلوب زيادتها")]
        public int NumberOfSeats { get; set; }
    }
}
