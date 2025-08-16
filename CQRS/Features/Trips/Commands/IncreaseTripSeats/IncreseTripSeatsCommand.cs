using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.IncreaseTripSeats
{
    public record IncreaseTripSeatsCommand(IncreaseTripSeatsDto SeatsDto) : IRequest<Response<string>>;

}
