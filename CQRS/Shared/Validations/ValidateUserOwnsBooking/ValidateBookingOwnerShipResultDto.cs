using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateUserOwnsBooking
{
    public class ValidateBookingOwnerShipResultDto
    {
        public int TripId { get; set; }
        public int NumberOfSeats { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}
