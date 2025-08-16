using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripForEnding
{
    public record ValidateTripForEndingCommand(int TripId, string DriverId) : IRequest<Response<Trip>>;

}
