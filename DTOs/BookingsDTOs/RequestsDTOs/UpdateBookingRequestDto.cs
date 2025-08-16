using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.BookingsDTOs.RequestsDTOs
{
    public class UpdateBookingRequestDto
    {
        [Required(ErrorMessage = "معرّف الرحلة مطلوب.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "معرّف الحجز مطلوب.")]
        public int BookingId { get; set; }

        //[Required(ErrorMessage = "معرّف المستخدم مطلوب.")]
        //public string UserId { get; set; }

        [Required(ErrorMessage = "عدد المقاعد الجديد مطلوب.")]
        [Range(1, 150, ErrorMessage = "يجب أن يكون عدد المقاعد بين 1 و150.")]
        public int NewNumberOfSeats { get; set; }
    }
}
