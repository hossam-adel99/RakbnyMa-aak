using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Tracking.SendAndStoreDriverLocation
{
    public record SendAndStoreDriverLocationCommand(int TripId, double Lat, double Lng, string driverId) : IRequest<Response<bool>>;
}