namespace RakbnyMa_aak.DTOs.Shared
{
    public class TripReportDto
    {
        public int TripId { get; set; }
        public string DriverFullName { get; set; }
        public string FromCityName { get; set; }
        public string FromGovernorateName { get; set; }
        public string ToCityName { get; set; }
        public string ToGovernorateName { get; set; }
        public DateTime TripDate { get; set; }
        public int AvailableSeats { get; set; }
        public decimal PricePerSeat { get; set; }
        public string TripStatus { get; set; }
        public bool WomenOnly { get; set; }
        public bool IsRecurring { get; set; }
    }


}
