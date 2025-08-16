using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Users.BookingTrip
{
    public class BookTripDto
    {
        [Required(ErrorMessage = "Trip ID is required.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "Number of seats is required.")]
        [Range(1, 150, ErrorMessage = "Number of seats must be between 1 and 150.")]
        public int NumberOfSeats { get; set; }

        [Required(ErrorMessage = "Passenger ID is required.")]
        public string PassengerUserId { get; set; } = null!;

        [Required(ErrorMessage = "Payment method is required.")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "Cash";
    }
}
