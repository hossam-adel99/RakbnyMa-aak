using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.PersistTrip
{
    public record PersistTripCommand(TripRequestDto TripDto) : IRequest<Response<int>>;
}
