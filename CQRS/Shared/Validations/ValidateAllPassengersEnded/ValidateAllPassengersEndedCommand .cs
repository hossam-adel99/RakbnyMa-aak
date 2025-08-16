using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateAllPassengersEnded
{
    public record ValidateAllPassengersEndedCommand(int TripId) : IRequest<Response<bool>>;

}
