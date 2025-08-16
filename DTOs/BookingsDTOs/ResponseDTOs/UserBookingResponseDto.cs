using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs
{
    public class UserBookingResponseDto
    {
        public int BookingId { get; set; }
        public int TripId { get; set; }
        public string DriverFullName { get; set; }
        public string DriverPicture { get; set; }
        public string DriverRate { get; set; }
        public string FromCityName { get; set; }
        public string ToCityName { get; set; }
        public DateTime TripDate { get; set; }
        public RequestStatus BookingStatus { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
