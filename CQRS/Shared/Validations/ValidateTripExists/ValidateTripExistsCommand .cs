using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists
{
    public record ValidateTripExistsCommand(int TripId) : IRequest<Response<TripValidationResultDto>>;


}
