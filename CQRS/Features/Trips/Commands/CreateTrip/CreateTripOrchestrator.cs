using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.CreateTrip
{
    public record CreateTripOrchestrator(TripRequestDto TripDto, string driverId) : IRequest<Response<int>>;
}
