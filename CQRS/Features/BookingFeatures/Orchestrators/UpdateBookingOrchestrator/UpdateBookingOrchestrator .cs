using MediatR;
using RakbnyMa_aak.DTOs.BookingsDTOs.RequestsDTOs;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.UpdateBookingOrchestrator
{
    public record UpdateBookingOrchestrator(UpdateBookingRequestDto BookingDto, string userId)
    : IRequest<Response<UpdateBookingSeatsResponseDto>>;

}
