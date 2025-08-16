namespace RakbnyMa_aak.DTOs.UserDTOs
{
    public class UserResponseDto
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int TotalTrips { get; set; }
        public double AverageRating { get; set; }
    }
}
