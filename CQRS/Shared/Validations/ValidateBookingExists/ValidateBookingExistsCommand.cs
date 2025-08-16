using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingExists
{
    public record ValidateBookingExistsCommand(int BookingId) : IRequest<Response<Booking>>;

}
