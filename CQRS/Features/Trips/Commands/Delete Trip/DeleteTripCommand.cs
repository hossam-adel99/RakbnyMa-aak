using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Trips.Delete_Trip
{
    public record DeleteTripCommand(int TripId, string CurrentUserId) : IRequest<Response<string>>;
}
