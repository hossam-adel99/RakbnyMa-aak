namespace RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs
{
    public class TripPassengersDto
    {
        public string PickupLocation { get; set; }
        public DateTime TripDate { get; set; }
        public int TotalPassengers { get; set; }
        public double AveragePassengerRating { get; set; }
        public List<PassengerInTripDto> Passengers { get; set; }
    }
}
