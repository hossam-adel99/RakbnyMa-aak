using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs
{
    public class CreateBookingResponseDto
    {
        public int BookingId { get; set; }
        public string PassengerId { get; set; }
        public int TripId { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal PricePerSeat { get; set; }
        public decimal TotalPrice { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
