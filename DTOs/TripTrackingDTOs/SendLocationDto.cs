using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.TripTrackingDTOs
{
    public class SendLocationDto
    {
        [Required(ErrorMessage = "TripId مطلوب.")]
        [Range(1, int.MaxValue, ErrorMessage = "TripId يجب أن يكون رقمًا صحيحًا أكبر من صفر.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "خط العرض (Lat) مطلوب.")]
        [Range(-90, 90, ErrorMessage = "Lat يجب أن يكون بين -90 و 90.")]
        public double Lat { get; set; }

        [Required(ErrorMessage = "خط الطول (Lng) مطلوب.")]
        [Range(-180, 180, ErrorMessage = "Lng يجب أن يكون بين -180 و 180.")]
        public double Lng { get; set; }
    }
}
