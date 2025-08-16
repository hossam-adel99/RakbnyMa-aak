using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingExists;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripOwner;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.BookValidationOrchestrator
{
    public class BookingValidationCommandHandler
        : IRequestHandler<BookingValidationOrchestrator, Response<BookingValidationResultDto>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BookingValidationCommandHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Response<BookingValidationResultDto>> Handle(BookingValidationOrchestrator request, CancellationToken cancellationToken)
        {
            // Step 1: Validate that the booking exists
            var bookingResult = await _mediator.Send(new ValidateBookingExistsCommand(request.BookingId));
            if (!bookingResult.IsSucceeded)
                return Response<BookingValidationResultDto>.Fail("هذا الحجز غير موجود.");

            var booking = bookingResult.Data;

            // Step 2: Validate that the associated trip exists
            var tripResult = await _mediator.Send(new ValidateTripExistsCommand(booking.TripId));
            if (!tripResult.IsSucceeded)
                return Response<BookingValidationResultDto>.Fail("هذه الرحلة غير موجودة.");

            var trip = tripResult.Data;

            // Step 3: Validate that the current user is the owner (driver) of the trip
            var ownerResult = await _mediator.Send(new ValidateTripOwnerCommand(request.CurrentUserId, trip.DriverId));
            if (!ownerResult.IsSucceeded)
                return Response<BookingValidationResultDto>.Fail("ليس لديك الصلاحية لإدارة هذا الحجز.");

            // Step 4: Check that booking seats ≤ available seats
            if (booking.NumberOfSeats > trip.AvailableSeats)
                return Response<BookingValidationResultDto>.Fail("عدد المقاعد المحجوزة يتجاوز عدد المقاعد المتاحة.");

            // Step 5: Return the validated data in a DTO
            var dto = _mapper.Map<BookingValidationResultDto>((booking, trip));
            return Response<BookingValidationResultDto>.Success(dto);
        }
    }
}
