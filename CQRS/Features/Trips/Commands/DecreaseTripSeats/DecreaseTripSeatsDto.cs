using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.DecreaseTripSeats
{
    public class DecreaseTripSeatsDto
    {
        [Required(ErrorMessage = "رقم الرحلة مطلوب.")]
        [Display(Name = "رقم الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "عدد المقاعد مطلوب.")]
        [Range(1, 150, ErrorMessage = "يجب أن يكون عدد المقاعد على الأقل 1.")]
        [Display(Name = "عدد المقاعد المراد تقليصها")]
        public int NumberOfSeats { get; set; }
    }
}
