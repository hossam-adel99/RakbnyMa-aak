using MediatR;
using RakbnyMa_aak.DTOs.BookingsDTOs.Requests;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.BookTripRequest
{
    public record BookTripRequestOrchestrator(BookTripRequestDto BookingDto, string userId) : IRequest<Response<int>>;
}
