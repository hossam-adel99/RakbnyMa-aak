using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripIsUpdatable
{
    public record ValidateTripIsUpdatableCommand(int TripId) : IRequest<Response<Trip>>;
}
