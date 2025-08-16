using MediatR;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.DecreaseBookingSeats
{
    public record DecreaseBookingSeatsCommand(UpdateBookingSeatsRequestDto Dto) : IRequest<Response<int>>;

}
