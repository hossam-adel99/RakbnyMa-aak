using MediatR;
using RakbnyMa_aak.DTOs.BookingsDTOs.RequestsDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.ApproveBookingRequest
{
    public record ApproveBookingOrchestrator(HandleBookingRequestDto Dto) : IRequest<Response<bool>>;
}
