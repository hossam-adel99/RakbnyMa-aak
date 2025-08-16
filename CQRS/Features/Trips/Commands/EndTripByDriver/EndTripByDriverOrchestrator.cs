using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.EndTripByDriver
{
    public record EndTripByDriverOrchestrator(int TripId, string DriverId) : IRequest<Response<bool>>;
}
