using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Trips.Queries
{
    public record GetDriverIdByTripIdQuery(int TripId) : IRequest<Response<string?>>;
}
