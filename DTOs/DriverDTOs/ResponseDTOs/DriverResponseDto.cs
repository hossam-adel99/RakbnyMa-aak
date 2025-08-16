using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs
{
    public class DriverResponseDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }

        public string CarModel { get; set; }
        public CarType CarType { get; set; }
        public CarColor CarColor { get; set; }
        public int CarCapacity { get; set; }
        public string CarPlateNumber { get; set; }
        public string DriverLicenseImage { get; set; }
        public string CarLicenseImage { get; set; }
        public int TotalTrips { get; set; }
        public double AverageRating { get; set; }
    }
}
