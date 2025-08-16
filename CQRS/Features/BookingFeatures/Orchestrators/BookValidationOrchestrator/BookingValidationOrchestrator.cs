using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.BookValidationOrchestrator
{
    public record BookingValidationOrchestrator(int BookingId, string CurrentUserId)
        : IRequest<Response<BookingValidationResultDto>>;
}
