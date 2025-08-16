using MediatR;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.IncreaseBookingSeats
{
    public record IncreaseBookingSeatsCommand(UpdateBookingSeatsRequestDto Dto) : IRequest<Response<int>>;
}
