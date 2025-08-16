using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateUserOwnsBooking
{
    public record ValidateUserOwnsBookingCommand(int BookingId, string UserId) : IRequest<Response<ValidateBookingOwnerShipResultDto>>;

}
