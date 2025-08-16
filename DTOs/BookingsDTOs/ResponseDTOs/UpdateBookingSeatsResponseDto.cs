namespace RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs
{
    public class UpdateBookingSeatsResponseDto
    {
        public int BookingId { get; set; }
        public int OldSeats { get; set; }
        public int NewSeats { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
