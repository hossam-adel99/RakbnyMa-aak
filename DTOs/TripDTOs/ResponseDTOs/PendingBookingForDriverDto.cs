namespace RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs
{
    public class PendingBookingForDriverDto
    {
        public int BookingId { get; set; }
        public int TripId { get; set; }
        public string PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string PassengerPicture { get; set; }
        public string FromCity { get; set; }
        public string FromGovernorate { get; set; }
        public string ToCity { get; set; }
        public string ToGovernorate { get; set; }
        public DateTime TripDate { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
