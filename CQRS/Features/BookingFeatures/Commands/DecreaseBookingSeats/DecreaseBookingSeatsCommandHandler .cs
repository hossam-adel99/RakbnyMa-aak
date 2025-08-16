using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.DecreaseBookingSeats
{
    public class DecreaseBookingSeatsCommandHandler
        : IRequestHandler<DecreaseBookingSeatsCommand, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DecreaseBookingSeatsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(
            DecreaseBookingSeatsCommand request,
            CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(request.Dto.BookingId);
            if (booking == null || booking.IsDeleted)
                return Response<int>.Fail("لم يتم العثور على الحجز.");

            // Update trip and booking
            booking.NumberOfSeats -= request.Dto.SeatsToChange;

            booking.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.BookingRepository.Update(booking);

            await _unitOfWork.CompleteAsync();

            return Response<int>.Success(booking.Id, "تم تقليل عدد مقاعد الحجز بنجاح.");
        }
    }
}
