using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripPassengers
{
    public record GetTripPassengersQuery(int TripId, string DriverUserId) : IRequest<Response<TripPassengersDto>>;

}
