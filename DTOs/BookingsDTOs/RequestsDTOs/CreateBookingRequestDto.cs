using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.BookingsDTOs.Requests
{
    public class CreateBookingRequestDto
    {
        [Required(ErrorMessage = "معرّف الرحلة مطلوب.")]
        [Display(Name = "معرّف الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "معرّف المستخدم مطلوب.")]
        [Display(Name = "معرّف المستخدم")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "عدد المقاعد مطلوب.")]
        [Range(1, 150, ErrorMessage = "يجب أن يكون عدد المقاعد بين 1 و150.")]
        [Display(Name = "عدد المقاعد")]
        public int NumberOfSeats { get; set; }

        [Required(ErrorMessage = "سعر المقعد مطلوب.")]
        [Range(1, double.MaxValue, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        [Display(Name = "سعر المقعد")]
        public decimal PricePerSeat { get; set; }
    }
}
