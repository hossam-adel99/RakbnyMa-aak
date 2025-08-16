using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.PreventDriverSelfBooking
{
    public record PreventDriverSelfBookingCommand(string DriverId, string UserId) : IRequest<Response<string>>;

}
