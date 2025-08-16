namespace RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs
{
    public class ScheduledTripRequestDto
    {
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
