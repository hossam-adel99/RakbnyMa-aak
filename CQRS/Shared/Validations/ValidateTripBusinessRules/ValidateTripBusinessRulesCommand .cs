using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripBusinessRules
{
    public record ValidateTripBusinessRulesCommand(TripRequestDto Trip) : IRequest<Response<bool>>;
}
