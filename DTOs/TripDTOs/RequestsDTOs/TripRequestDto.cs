using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs
{
    public class TripRequestDto
    {
        public string? DriverId { get; set; }

        [Required]
        [Display(Name = "المدينة من")]
        public int FromCityId { get; set; }

        [Required]
        [Display(Name = "المدينة إلى")]
        public int ToCityId { get; set; }

        [Required]
        [Display(Name = "المحافظة من")]
        public int FromGovernorateId { get; set; }

        [Required]
        [Display(Name = "المحافظة إلى")]
        public int ToGovernorateId { get; set; }

        [Required(ErrorMessage = "موقع الانطلاق مطلوب.")]
        [Display(Name = "موقع الانطلاق")]
        public string PickupLocation { get; set; }

        [Required(ErrorMessage = "موقع الوصول مطلوب.")]
        [Display(Name = "موقع الوصول")]
        public string DestinationLocation { get; set; }

        [Required(ErrorMessage = "تاريخ الرحلة مطلوب.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ الرحلة")]
        public DateTime TripDate { get; set; }

        [Required(ErrorMessage = "عدد المقاعد المتاحة مطلوب.")]
        [Range(1, 150, ErrorMessage = "عدد المقاعد المتاحة يجب أن يكون بين 1 و150.")]
        [Display(Name = "عدد المقاعد المتاحة")]
        public int AvailableSeats { get; set; }

        [Required(ErrorMessage = "سعر المقعد مطلوب.")]
        [Range(1, 10000, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        [Display(Name = "سعر المقعد")]
        public decimal PricePerSeat { get; set; }

        [Display(Name = "هل الرحلة متكررة؟")]
        public bool IsRecurring { get; set; }

        [Display(Name = "للنساء فقط؟")]
        public bool WomenOnly { get; set; }
    }
}
