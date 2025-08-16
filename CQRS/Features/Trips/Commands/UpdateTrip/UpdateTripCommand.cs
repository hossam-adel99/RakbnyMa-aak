using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.UpdateTrip
{
    public record UpdateTripCommand(int TripId, TripRequestDto TripDto) : IRequest<Response<int>>;

}
