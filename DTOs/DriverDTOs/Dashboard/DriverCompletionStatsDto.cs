namespace RakbnyMa_aak.DTOs.DriverDTOs.Dashboard
{
    public class DriverCompletionStatsDto
    {
        public double OverallCompletionRate { get; set; }
        public List<MonthlyCompletionRateDto> MonthlyRates { get; set; }
    }
}
