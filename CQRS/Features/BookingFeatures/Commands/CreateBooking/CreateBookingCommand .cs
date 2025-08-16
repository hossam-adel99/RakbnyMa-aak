using MediatR;
using RakbnyMa_aak.DTOs.BookingsDTOs.Requests;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.CreateBooking
{
    public record CreateBookingCommand(CreateBookingRequestDto BookingDto) : IRequest<Response<int>>;

}
