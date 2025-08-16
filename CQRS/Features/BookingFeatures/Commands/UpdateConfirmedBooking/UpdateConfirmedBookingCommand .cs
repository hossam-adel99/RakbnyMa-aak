using MediatR;
using RakbnyMa_aak.DTOs.BookingsDTOs.RequestsDTOs;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.UpdateConfirmedBooking
{
    public record UpdateConfirmedBookingCommand(
            UpdateBookingRequestDto BookingDto,
            int OldSeats,
            string userId
        ) : IRequest<Response<UpdateBookingSeatsResponseDto>>;
}
