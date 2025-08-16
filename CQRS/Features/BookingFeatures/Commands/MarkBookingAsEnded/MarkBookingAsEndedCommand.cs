using MediatR;
using RakbnyMa_aak.GeneralResponse;
namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.MarkBookingAsEnded
{
    public record MarkBookingAsEndedCommand(int BookingId) : IRequest<Response<bool>>;

}
