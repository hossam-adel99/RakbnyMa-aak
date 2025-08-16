using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Models
{
    public class Trip : BaseEntity
    {
        // Id is inherited from BaseEntity (int PK)
        [Required(ErrorMessage = "معرف السائق مطلوب.")]
        [Display(Name = "معرف السائق")]
        public string DriverId { get; set; } // FK to Driver (string, as Driver.UserId is string)

        // City and Governorate can be strings or separate lookup entities
        // Keeping them as strings for now as per the ERD's attributes

        [Required]
        [Display(Name = "من المدينة")]
        public int FromCityId { get; set; }

        [Required]
        [Display(Name = "إلى المدينة")]
        public int ToCityId { get; set; }

        [Required]
        [Display(Name = "من المحافظة")]
        public int FromGovernorateId { get; set; }

        [Required]
        [Display(Name = "إلى المحافظة")]
        public int ToGovernorateId { get; set; }

        [Required(ErrorMessage = "موقع الالتقاء مطلوب.")]
        [Display(Name = "موقع الالتقاء")]
        public string PickupLocation { get; set; }

        [Required(ErrorMessage = "موقع الوصول مطلوب.")]
        [Display(Name = "موقع الوصول")]
        public string DestinationLocation { get; set; }

        [Required(ErrorMessage = "تاريخ الرحلة مطلوب.")]
        [Display(Name = "تاريخ الرحلة")]
        public DateTime TripDate { get; set; }

        [Required(ErrorMessage = "عدد المقاعد المتاحة مطلوب.")]
        [Range(1, 150, ErrorMessage = "يجب أن يكون عدد المقاعد المتاحة على الأقل 1.")]
        [Display(Name = "المقاعد المتاحة")]
        public int AvailableSeats { get; set; }

        [Required(ErrorMessage = "سعر المقعد مطلوب.")]
        [Range(1, double.MaxValue, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        [Display(Name = "سعر المقعد")]
        public decimal PricePerSeat { get; set; }

        [Required]
        [Display(Name = "حالة الرحلة")]
        public TripStatus TripStatus { get; set; }

        [Display(Name = "هل الرحلة متكررة؟")]
        public bool IsRecurring { get; set; } = false;

        [Display(Name = "هل للنساء فقط؟")]
        public bool WomenOnly { get; set; } = false;

        [Display(Name = "من المدينة")]
        public City FromCity { get; set; }

        [Display(Name = "إلى المدينة")]
        public City ToCity { get; set; }

        [Display(Name = "من المحافظة")]
        public Governorate FromGovernorate { get; set; }

        [Display(Name = "إلى المحافظة")]
        public Governorate ToGovernorate { get; set; }

        // Navigation property to Driver
        [ForeignKey("DriverId")]
        [Display(Name = "السائق")]
        public virtual Driver Driver { get; set; }

        // Trip has many Bookings
        [Display(Name = "الحجوزات")]
        public virtual ICollection<Booking> Bookings { get; set; }

        // Trip has many Ratings (ratings for this specific trip)
        [Display(Name = "التقييمات")]
        public virtual ICollection<Rating> Ratings { get; set; }

        // Trip has many Tracking points
        [Display(Name = "تتبع الرحلة")]
        public virtual ICollection<TripTracking> TripTrackings { get; set; }

        public Trip()
        {
            Bookings = new HashSet<Booking>();
            Ratings = new HashSet<Rating>();
            TripTrackings = new HashSet<TripTracking>();
        }
    }
}
