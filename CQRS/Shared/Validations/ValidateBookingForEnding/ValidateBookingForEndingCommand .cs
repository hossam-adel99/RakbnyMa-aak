using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingForEnding
{
    public record ValidateBookingForEndingCommand(int BookingId, string CurrentUserId) : IRequest<Response<ValidateBookingDto>>;

}
