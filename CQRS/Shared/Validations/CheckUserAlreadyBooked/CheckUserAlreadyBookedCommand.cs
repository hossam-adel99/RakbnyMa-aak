using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.CheckUserAlreadyBooked
{
    public record CheckUserAlreadyBookedCommand(CheckUserAlreadyBookedDto CheckUserAlreadyBookedDto) : IRequest<Response<bool>>;

}
