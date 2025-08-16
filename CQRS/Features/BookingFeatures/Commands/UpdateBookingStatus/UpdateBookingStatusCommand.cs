using MediatR;
using RakbnyMa_aak.GeneralResponse;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.UpdateBookingStatus
{
    public record UpdateBookingStatusCommand(int BookingId, int TripId, RequestStatus NewStatus) : IRequest<Response<bool>>;

}
