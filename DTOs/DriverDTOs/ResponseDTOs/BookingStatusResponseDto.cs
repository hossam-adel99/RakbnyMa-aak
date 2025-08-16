namespace RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs
{
    public class BookingStatusResponseDto
    {
        public int TripID { get; set; }
        public int BookingId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
       
        public DateTime RequestDate { get; set; }
    }
}
