using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Orchestrators.ValidateTripBusinessLogic
{
    public record ValidateTripBusinessLogicOrchestrator(TripRequestDto TripDto) : IRequest<Response<bool>>;

}
