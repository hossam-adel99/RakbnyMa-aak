using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.CheckTimeBeforeTrip
{
    public record CheckTimeBeforeTripCommand(DateTime StartDateTime, int MinimumHours=3) : IRequest<Response<bool>>;
}
