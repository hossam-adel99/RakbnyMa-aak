using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripOwner
{
    public record ValidateTripOwnerCommand(string CurrentUserId, string DriverId) : IRequest<Response<bool>>;

}
