namespace RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs
{
    public class TripResponseDto
    {
        public int Id { get; set; }
        public string DriverFullName { get; set; }
        public string DriverPicture { get; set; }
        public string DriverRate { get; set; }
        public string PickupLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime TripDate { get; set; }
        public int AvailableSeats { get; set; }
        public decimal PricePerSeat { get; set; }
        public string FromCityName { get; set; }
        public string ToCityName { get; set; }
        public string FromGovernorateName { get; set; }
        public string ToGovernorateName { get; set; }
        public string TripStatus { get; set; }
        public string CarModel { get; set; }
        public bool IsRecurring { get; set; }
        public bool WomenOnly { get; set; }
    }
}
