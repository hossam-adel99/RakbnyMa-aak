using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.EndTripByPassenger
{
    public record EndTripByPassengerCommand(int BookingId, string CurrentUserId) : IRequest<Response<bool>>;

}
