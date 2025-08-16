using MediatR;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.CancelBookingValidationOrchestrator;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.IncreaseTripSeats;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.CancelBookingByPassenger
{
    public class CancelBookingByPassengerHandler : IRequestHandler<CancelBookingByPassengerCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CancelBookingByPassengerHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response<bool>> Handle(CancelBookingByPassengerCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Validate and get info
            var validationResult = await _mediator.Send(new CancelBookingValidationOrchestrator(request.bookingId, request.userId));
            if (!validationResult.IsSucceeded)
                return Response<bool>.Fail(validationResult.Message);

            var dto = validationResult.Data;

            // Step 2: Load the booking directly (we only need to update status)
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(dto.BookingId);
            if (booking is null)
                return Response<bool>.Fail("لم يتم العثور على الحجز.");

            booking.RequestStatus = RequestStatus.Cancelled;
            booking.HasEnded = true;
            booking.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.BookingRepository.Update(booking);

            // Step 3: Restore seats if booking was confirmed // Because only confirmed bookings affect trip seats
            if (dto.WasConfirmed)
            {
                var increaseTripDto = new IncreaseTripSeatsDto
                {
                    TripId = dto.TripId,
                    NumberOfSeats = dto.NumberOfSeats
                };
                await _mediator.Send(new IncreaseTripSeatsCommand(increaseTripDto));
            }

            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم إلغاء الحجز بنجاح.");
        }

    }

}
