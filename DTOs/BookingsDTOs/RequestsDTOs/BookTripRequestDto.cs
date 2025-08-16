using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.BookingsDTOs.Requests
{
    public class BookTripRequestDto
    {
        public string? UserId { get; set; }

        [Required(ErrorMessage = "معرّف الرحلة مطلوب.")]
        [Display(Name = "معرّف الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "عدد المقاعد مطلوب.")]
        [Range(1, 150, ErrorMessage = "يجب أن يكون عدد المقاعد بين 1 و150.")]
        [Display(Name = "عدد المقاعد")]
        public int NumberOfSeats { get; set; }

    }
}
