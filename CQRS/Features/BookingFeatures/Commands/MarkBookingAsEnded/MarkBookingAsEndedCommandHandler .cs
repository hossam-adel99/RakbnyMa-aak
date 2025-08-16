using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.MarkBookingAsEnded
{
    public class MarkBookingAsEndedCommandHandler : IRequestHandler<MarkBookingAsEndedCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkBookingAsEndedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(MarkBookingAsEndedCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(request.BookingId);
            if (booking == null || booking.IsDeleted || booking.RequestStatus == RequestStatus.Cancelled)
                return Response<bool>.Fail("لم يتم العثور على الحجز أو تم إلغاؤه أو حذفه.");

            booking.HasEnded = true;

            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم تمييز الحجز بأنه منتهٍ.");
        }
    }
}
