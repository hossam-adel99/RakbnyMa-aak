using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.CompleteTrip
{
    public record CompleteTripCommand(int TripId) : IRequest<Response<bool>>;

}
