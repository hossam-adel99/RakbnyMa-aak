using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.StartTripByPassenger
{
    public record StartTripByPassengerCommand(int BookingId, string CurrentUserId) : IRequest<Response<bool>>;

}
