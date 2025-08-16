using MediatR;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateOwnershipAndGetBooking;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists;
using RakbnyMa_aak.GeneralResponse;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.CancelBookingValidationOrchestrator
{
    public class CancelBookingValidationOrchestratorHandler
      : IRequestHandler<CancelBookingValidationOrchestrator, Response<CancelBookingValidationResultDto>>
    {
        private readonly IMediator _mediator;

        public CancelBookingValidationOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<CancelBookingValidationResultDto>> Handle(CancelBookingValidationOrchestrator request, CancellationToken cancellationToken)
        {
            // Step 1: Validate that the user owns the booking and retrieve the booking details
            var ownershipResult = await _mediator.Send(new ValidateOwnershipAndGetBookingCommand(request.BookingId, request.UserId));
            if (!ownershipResult.IsSucceeded)
                return Response<CancelBookingValidationResultDto>.Fail(ownershipResult.Message);

            var booking = ownershipResult.Data!;

            // Step 2: Validate that the trip associated with the booking exists
            var tripResult = await _mediator.Send(new ValidateTripExistsCommand(booking.TripId));
            if (!tripResult.IsSucceeded)
                return Response<CancelBookingValidationResultDto>.Fail(tripResult.Message);

            var trip = tripResult.Data;

            // Step 3: Check that the trip is not starting within the next 3 hours
            if (trip.TripDate <= DateTime.UtcNow.AddHours(3))
                return Response<CancelBookingValidationResultDto>.Fail("يمكنك إلغاء الحجز فقط قبل بدء الرحلة بثلاث ساعات على الأقل.");

            return Response<CancelBookingValidationResultDto>.Success(new CancelBookingValidationResultDto
            {
                BookingId = request.BookingId,
                TripId = booking.TripId,
                NumberOfSeats = booking.NumberOfSeats,
                WasConfirmed = booking.RequestStatus == RequestStatus.Confirmed,
            });
        }
    }

}
