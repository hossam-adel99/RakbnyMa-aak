namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingForEnding
{
    public class ValidateBookingDto
    {
        public int Id { get; set; }
        public bool HasStarted { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
