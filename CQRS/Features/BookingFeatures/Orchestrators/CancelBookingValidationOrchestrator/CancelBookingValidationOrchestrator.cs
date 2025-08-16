using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.CancelBookingValidationOrchestrator
{
    public record CancelBookingValidationOrchestrator(int BookingId, string UserId)
      : IRequest<Response<CancelBookingValidationResultDto>>;

}
