using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Orchestrators.UpdateTrip
{
    public record UpdateTripOrchestrator(int TripId, string CurrentUserId, TripRequestDto TripDto) : IRequest<Response<int>>;

}
