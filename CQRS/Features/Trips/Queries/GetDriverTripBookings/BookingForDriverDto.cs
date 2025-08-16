using System;
using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Trip.Queries.GetDriverTripBookings
{
    public class BookingForDriverDto
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required]
        [Display(Name = "اسم الراكب")]
        [StringLength(100, ErrorMessage = "يجب ألا يزيد الاسم عن 100 حرف.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "حالة الحجز")]
        public string Status { get; set; }

        [Display(Name = "تاريخ طلب الحجز")]
        public DateTime RequestDate { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
