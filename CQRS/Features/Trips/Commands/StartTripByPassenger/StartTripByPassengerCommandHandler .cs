using MediatR;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingExists;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;
namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.StartTripByPassenger
{
    public class StartTripByPassengerHandler : IRequestHandler<StartTripByPassengerCommand, Response<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public StartTripByPassengerHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(StartTripByPassengerCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ValidateBookingExistsCommand(request.BookingId));
            if (!result.IsSucceeded)
                return Response<bool>.Fail(result.Message);

            var booking = result.Data;

            if (booking.UserId != request.CurrentUserId)
                return Response<bool>.Fail("راكب غير مصرح له.");

            if (booking.HasStarted)
                return Response<bool>.Fail("بدأ هذا الراكب الرحلة مسبقًا.");

            var trip = await _unitOfWork.TripRepository.GetByIdAsync(booking.TripId);
            if (trip == null || trip.IsDeleted)
                return Response<bool>.Fail("لم يتم العثور على الرحلة.");

            if (trip.TripStatus != TripStatus.Ongoing)
                return Response<bool>.Fail("يمكنك بدء رحلتك فقط عندما تكون الرحلة في حالة جارية.");

            booking.HasStarted = true;
            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "بدأ الراكب الرحلة.");
        }
    }
}
